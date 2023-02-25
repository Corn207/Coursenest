import { faChevronDown, faChevronRight, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useContext, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import CourseContext from '~/contexts/courseContext';

import styles from './EditLesson.module.scss';

const cx = classNames.bind(styles);

function EditLesson() {
    const [lessons, setLessons] = useState([]);
    const { courseData, setCourseData } = useContext(CourseContext);

    const navigate = useNavigate();
    let params = useParams();

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
        if (lessons.length === 0) {
            const defaultNewLesson = {
                LessonId: 1,
                Title: `New item 1`,
                Description: 'Description of item ',
            };
            const addedLessonsList = [defaultNewLesson];
            setLessons(addedLessonsList);
        } else {
            const defaultNewLesson = {
                LessonId: lessons[lessons.length - 1].LessonId + 1,
                Title: `New item ${lessons[lessons.length - 1].LessonId + 1}`,
                Description: 'Description of item ',
            };
            const addedLessonsList = [...lessons, defaultNewLesson];
            setLessons(addedLessonsList);
        }
        // setLessons(addedLessonsList);
        console.log(lessons);
    };

    const handleDeleteLesson = (id) => {
        const newArrLesson = [...lessons.filter((item) => item.LessonId !== id)];
        setLessons(newArrLesson);
        console.log(newArrLesson);
    };

    const handleEditLesson = (id) => {
        navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson/edit-material`);
    };

    const handleCancel = () => {
        navigate(`/publisher/${params.PublisherUserId}/add-course`);
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
        <div className={cx('wrapper')}>
            <div className={cx('topTitle')}>
                <p className={cx('courseTitle')}>{'Title Course'}</p>
                <div>
                    <FontAwesomeIcon className={cx('icon')} icon={faChevronRight} />
                </div>
                <p className={cx('lessonTitle')}>Lesson 1</p>
            </div>
            <div className={cx('lessonBody')}>
                <p className={cx('lessonTitleDetail')}>{'Lesson 1: How to...'} </p>
                <p className={cx('lessonDesc')}>{'This will teach you...'} </p>
                <div className={cx('unitsBody')}>
                    <div className={cx('unitsTopBody')}>
                        <p className={cx('unitsTitle')}>Units</p>
                        <div className={cx('unitsBtns')}>
                            <button className={cx('unitsBtn')} onClick={handleAddLessonClick}>
                                Add Material
                            </button>
                            <button className={cx('unitsBtn')}>Add Exam</button>
                        </div>
                    </div>
                    <ul className={cx('listWrapper')}>
                        {lessons.map((item, index) => (
                            <li className={cx('itemDiv')} key={index}>
                                <p className={cx('itemTitle')}>{item.Title}</p>
                                <div className={cx('itemAction')}>
                                    <p className={cx('btnAction')} onClick={handleEditLesson}>
                                        Edit
                                    </p>
                                    <p className={cx('btnAction')} onClick={() => handleDeleteLesson(item.LessonId)}>
                                        Delete
                                    </p>
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
            </div>
            <CancelConfirmBtns onCancel={handleCancel} />
        </div>
    );
}

export default EditLesson;
