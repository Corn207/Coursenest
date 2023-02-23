import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './LessonsMaterialLists.module.scss';

const cx = classNames.bind(styles);

function LessonsMaterialLists() {
    // const [lessons, setLessons] = useState(lessonsList);
    const [lessons, setLessons] = useState([]);

    // const [lessonsList, setLessonsList] = useState([]);

    const moveItem = (LessonId, direction) => {
        const newItems = [...lessons];
        const index = newItems.findIndex((item) => item.LessonId === LessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setLessons(newItems);
    };

    const handleAddLessonClick = () => {
        const defaultNewLesson = {
            LessonId: lessons.length + 1,
            Title: 'New Lesson',
            Description: 'Description of lesson ',
        };
        const addedLessonsList = [...lessons, defaultNewLesson];

        setLessons(addedLessonsList);
        console.log(lessons);
    };

    let activeBtn = {
        opacity: '1',
        cursor: 'pointer',
    };

    let disableBtn = {
        opacity: '0.3',
        cursor: 'not-allowed',
    };

    return (
        <div className={cx('lessonsContainer')}>
            <div className={cx('topContainer')}>
                <p className={cx('topTitle')}>Lessons</p>
                <button className={cx('topTitleBtn')} onClick={handleAddLessonClick}>
                    {/* <Link className={cx('addLessonLink')} to="add-lesson"> */}
                    Add Lesson
                    {/* </Link> */}
                </button>
            </div>
            <ul className={cx('wrapper')}>
                {lessons.map((item, index) => (
                    <li className={cx('itemDiv')} key={index}>
                        <p className={cx('itemTitle')}>{item.Title}</p>
                        <div className={cx('itemAction')}>
                            <p className={cx('btnAction')}>Edit</p>
                            <p className={cx('btnAction')}>Delete</p>
                            <p className={cx('itemOrder')}>{index + 1}</p>
                            <div className={cx('moveBtnContainer')}>
                                {/* {lessons[lessons.indexOf(item) + 1] && (
                                <button onClick={() => moveItem(item.LessonId, 1)}>
                                    <FontAwesomeIcon icon={faChevronUp} />
                                </button>
                            )} */}
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) - 1] ? activeBtn : disableBtn}
                                    onClick={() =>
                                        lessons[lessons.indexOf(item) - 1]
                                            ? moveItem(item.LessonId, -1)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronUp} />
                                </button>
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) + 1] ? activeBtn : disableBtn}
                                    onClick={() =>
                                        lessons[lessons.indexOf(item) + 1]
                                            ? moveItem(item.LessonId, 1)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronDown} />
                                </button>

                                {/* {lessons[lessons.indexOf(item) - 1] && (
                                <button onClick={() => moveItem(item.LessonId, -1)}>
                                    <FontAwesomeIcon icon={faChevronDown} />
                                </button>
                            )} */}
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default LessonsMaterialLists;
