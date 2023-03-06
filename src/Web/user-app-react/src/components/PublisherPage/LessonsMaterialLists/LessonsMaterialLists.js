import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useEffect } from 'react';
import { useContext, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CourseContext from '~/contexts/courseContext';

import styles from './LessonsMaterialLists.module.scss';

const cx = classNames.bind(styles);

function LessonsMaterialLists({ lessonsList, handleNextStep, handleTitleValue, getLessonsListOnAdd, onEdit }) {
    const [lessons, setLessons] = useState(lessonsList);
    // const { lessons, setLessons } = useContext(CourseContext);

    // useEffect(() => {
    //     setLessons(lessonsList);
    // }, [lessonsList]);

    const navigate = useNavigate();
    let params = useParams();

    // const [lessonsList, setLessonsList] = useState([]);

    const moveItem = (LessonId, direction, event) => {
        event.preventDefault();
        const newItems = [...lessons];
        const index = newItems.findIndex((item) => item.LessonId === LessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setLessons(newItems);
    };

    const handleAddLessonClick = (event) => {
        event.preventDefault();
        if (lessons.length === 0) {
            const defaultNewLesson = {
                LessonId: 1,
                Title: `New item 1`,
                Description: 'Description of item ',
            };
            const addedLessonsList = [defaultNewLesson];
            setLessons(addedLessonsList);
            getLessonsListOnAdd(addedLessonsList);
        } else {
            const defaultNewLesson = {
                LessonId: lessons[lessons.length - 1].LessonId + 1,
                Title: `New item ${lessons[lessons.length - 1].LessonId + 1}`,
                Description: 'Description of item ',
            };
            const addedLessonsList = [...lessons, defaultNewLesson];
            setLessons(addedLessonsList);
            getLessonsListOnAdd(addedLessonsList);
        }
        // setLessons(addedLessonsList);
        console.log(lessons);
    };

    const handleDeleteLesson = (id) => {
        const newArrLesson = [...lessons.filter((item) => item.LessonId !== id)];
        setLessons(newArrLesson);
        console.log(newArrLesson);
    };

    const handleEditLesson = (lesson) => {
        onEdit(lesson);
        handleNextStep();
        handleTitleValue(lesson.Title);
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
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
                    <li className={cx('itemDiv')} key={item.LessonId}>
                        <p className={cx('itemTitle')}>{item.Title}</p>
                        <div className={cx('itemAction')}>
                            <p className={cx('btnAction')} onClick={() => handleEditLesson(item)}>
                                Edit
                            </p>
                            <p className={cx('btnAction')} onClick={() => handleDeleteLesson(item.LessonId)}>
                                Delete
                            </p>
                            <p className={cx('itemOrder')}>{index + 1}</p>
                            <div className={cx('moveBtnContainer')}>
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) - 1] ? activeBtn : disableBtn}
                                    onClick={(event) =>
                                        lessons[lessons.indexOf(item) - 1]
                                            ? moveItem(item.LessonId, -1, event)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronUp} />
                                </button>
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) + 1] ? activeBtn : disableBtn}
                                    onClick={(event) =>
                                        lessons[lessons.indexOf(item) + 1]
                                            ? moveItem(item.LessonId, 1, event)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronDown} />
                                </button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default LessonsMaterialLists;
