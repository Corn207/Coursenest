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
import { useDebounce } from '~/hooks';
import ChosenTopicsList from '../ChosenTopicsList';
import axios from 'axios';
import topicsApi from '~/api/topicsApi';

const cx = classNames.bind(styles);

function TopicsSearch({ handleTopicsId }) {
    // const [inputText, setInputText] = useState('');
    const [allCourses, setAllCourses] = useState([]);
    const [chosenCourses, setChosenCourses] = useState([]);
    const [chosenTopicsId, setChosenTopicsId] = useState([]);
    const [searchValue, setSearchValue] = useState('');
    const [searchResult, setSearchResult] = useState([]);
    const [searchResultFiltered, setSearchResultFiltered] = useState([]);
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
    useEffect(() => {});

    useEffect(() => {
        if (!debouncedValue.trim()) {
            // setSearchResult([]);
            return;
        }

        // const resultsArray = allCourses.filter(
        //     (post) =>
        //         post.title.includes(debouncedValue.toLowerCase()) || post.body.includes(debouncedValue.toLowerCase()),
        // );

        const fetchTopics = async () => {
            const response = await axios.get(
                `https://coursenest.corn207.loseyourip.com/topics?Content=${debouncedValue}`,
            );
            // if (chosenTopicsId.length !== 0 || chosenTopicsId !== undefined) {
            if (chosenTopicsId !== undefined) {
                await setSearchResultFiltered(response.data.filter((item) => !chosenTopicsId.includes(item.topicId)));
                // console.log(response.data.filter((item) => item.topicId !== chosenTopicsId[i]));
                // i++;
                // setSearchResult(searchResultFiltered);
            } else setSearchResultFiltered(response.data);
            // setSearchResult(searchResultFiltered);

            console.log(response.data);
        };
        fetchTopics();
        // setSearchResult(resultsArray);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debouncedValue]);

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

    const handleClick = async (courseId) => {
        const chosenCourse = await topicsApi.get(courseId);
        const newArr = searchResultFiltered.filter((item) => item.topicId !== courseId);
        // if (!Array.isArray(chosenTopicsId)) {
        //     setChosenTopicsId([...[chosenTopicsId]], courseId);
        // }

        // setChosenTopicsId([...[chosenTopicsId]], courseId);

        if (chosenTopicsId !== undefined) {
            setChosenTopicsId([courseId, ...chosenTopicsId]);
        } else setChosenTopicsId([courseId]);

        // setChosenTopicsId([courseId, ...chosenTopicsId]);

        setChosenArr([searchResultFiltered.findIndex((e) => e.topicId === courseId), ...chosenArr]);
        // setSearchValue('');
        setDropDown(false);
        setSearchResultFiltered(newArr);
        setChosenCourses((chosenCourses) => [chosenCourse, ...chosenCourses]);
        // setChosenTopicsId((chosenCourse) => [chosenCourse.topicId, ...chosenCourses.map((topic) => topic.topicId)]);
        handleTopicsId(chosenCourses);
        console.log(chosenCourses);
        console.log(chosenTopicsId);
    };

    const handleRemoveCourse = async (courseId) => {
        const chosenCourse = await topicsApi.get(courseId);

        const newArr = chosenCourses.filter((item) => item.topicId !== courseId);
        const leftSearchArr = searchResultFiltered.slice(0, chosenArr[0]);
        const rightSearchArr = searchResultFiltered.slice(chosenArr[0], searchResultFiltered.length);
        const searchArr = [...leftSearchArr, chosenCourse, ...rightSearchArr];

        await setSearchResultFiltered(searchArr);
        await setChosenCourses(newArr);
        setChosenTopicsId(...newArr.map((topic) => topic.topicId));

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
                                    courses={searchResultFiltered}
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
