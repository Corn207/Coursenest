import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useEffect, useRef, useState } from 'react';
import HeadlessTippy from '@tippyjs/react/headless';

import styles from './TopicsSearch.module.scss';
import images from '~/assets/images';
import Image from '~/components/Image';
import TopicsList from '../TopicsList/TopicsList';
import { Wrapper as PopperWrapper } from '~/components/Popper';
import coursesApi from '~/api/coursesApi';
import { useDebounce } from '~/hooks';
import ChosenTopicsList from '../ChosenTopicsList';

const cx = classNames.bind(styles);

function TopicsSearch({ handleTopicsId }) {
    // const [inputText, setInputText] = useState('');
    const [allCourses, setAllCourses] = useState([]);
    const [chosenCourses, setChosenCourses] = useState([]);
    const [searchValue, setSearchValue] = useState('');
    const [searchResult, setSearchResult] = useState([]);
    const [dropDown, setDropDown] = useState(false);
    const [chosenArr, setChosenArr] = useState([]);

    const debouncedValue = useDebounce(searchValue, 1000);

    const inputRef = useRef();

    // let favorCourses = [];

    // useEffect(() => {
    //     const fetchCourses = async () => {
    //         const TopicsList = await coursesApi.getAll();
    //         setAllCourses(TopicsList);
    //         setSearchResult(TopicsList);
    //     };

    //     fetchCourses();
    // }, []);

    useEffect(() => {
        if (!debouncedValue.trim()) {
            setSearchResult([]);
            return;
        }

        const resultsArray = allCourses.filter(
            (post) =>
                post.title.includes(debouncedValue.toLowerCase()) || post.body.includes(debouncedValue.toLowerCase()),
        );

        setSearchResult(resultsArray);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debouncedValue]);

    const handleClick = async (courseId) => {
        const chosenCourse = await coursesApi.get(courseId);
        const newArr = searchResult.filter((item) => item.id !== courseId);

        setChosenArr([searchResult.findIndex((e) => e.id === courseId), ...chosenArr]);
        setSearchValue('');
        setDropDown(false);
        setSearchResult(newArr);
        await setChosenCourses((chosenCourses) => [chosenCourse, ...chosenCourses]);
        handleTopicsId(chosenCourses);
    };

    const handleDropDownClick = () => {
        setDropDown(!dropDown);
    };

    const handleHideResult = () => {
        setDropDown(false);
    };

    const handleChange = (e) => {
        const searchValue = e.target.value;

        if (!searchValue.startsWith(' ')) {
            setSearchValue(e.target.value);
        }
    };

    const handleRemoveCourse = async (courseId) => {
        const chosenCourse = await coursesApi.get(courseId);

        const newArr = chosenCourses.filter((item) => item.id !== courseId);
        const leftSearchArr = searchResult.slice(0, chosenArr[0]);
        const rightSearchArr = searchResult.slice(chosenArr[0], searchResult.length);
        const searchArr = [...leftSearchArr, chosenCourse, ...rightSearchArr];

        await setSearchResult(searchArr);
        await setChosenCourses(newArr);
        handleTopicsId(chosenCourses);
    };

    return (
        // Fix tippyjs error by adding a wrapper <div> or <span>
        <div>
            <div>
                <HeadlessTippy
                    interactive
                    visible={dropDown}
                    placement={'bottom-start'}
                    render={(attrs) => (
                        <div className={cx('search-result')} tabIndex="-1" {...attrs}>
                            <PopperWrapper className={cx('popper-result')}>
                                <TopicsList
                                    className={cx('TopicsListDropDown')}
                                    courses={searchResult}
                                    onChose={handleClick}
                                />
                            </PopperWrapper>
                        </div>
                    )}
                    onClickOutside={handleHideResult}
                >
                    <div className={cx('wrapper')}>
                        <label>You want to study...</label>
                        <div className={cx('searchDiv')}>
                            <FontAwesomeIcon className={cx('searchIcon')} icon={faMagnifyingGlass} />
                            <input
                                ref={inputRef}
                                value={searchValue}
                                className={cx('searchInput')}
                                type="text"
                                placeholder="Search..."
                                onChange={handleChange}
                                onFocus={() => setDropDown(true)}
                            ></input>
                            <button className={cx('dropDownButton')} onClick={handleDropDownClick}>
                                <Image className={cx('dropDownImg')} src={images.dropDownIcon}></Image>
                            </button>
                        </div>
                    </div>
                </HeadlessTippy>
            </div>
            <ChosenTopicsList courses={chosenCourses} onChose={handleRemoveCourse} />
        </div>
    );
}

export default TopicsSearch;
