import classNames from 'classnames/bind';
import { useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import LessonsMaterialLists from '~/components/PublisherPage/LessonsMaterialLists';

import styles from './AddCourses.module.scss';

const cx = classNames.bind(styles);

function AddCourses() {
    const [image, setImage] = useState(null);
    const [isSelected, setIsSlected] = useState(1);
    const [lessonsList, setLessonsList] = useState([]);
    const [titleValue, setTitleValue] = useState('');
    const [error, setError] = useState('');

    let params = useParams();

    // const lessonsList = [
    //     { LessonId: 1, Title: 'Lesson 1', Description: 'Description of lesson 1', Order: 1.5 },
    //     { LessonId: 2, Title: 'Lesson 2', Description: 'Description of lesson 2', Order: 2.5 },
    //     { LessonId: 3, Title: 'Lesson 3', Description: 'Description of lesson 3', Order: 3.5 },
    //     { LessonId: 4, Title: 'Lesson 4', Description: 'Description of lesson 4', Order: 4.5 },
    //     { LessonId: 5, Title: 'Lesson 5', Description: 'Description of lesson 5', Order: 5.5 },
    // ];

    const handleChange = (e) => {
        setImage(URL.createObjectURL(e.target.files[0]));
    };

    const handleClick = (id) => {
        setIsSlected(id);
    };

    // const handleAddLessonClick = () => {
    //     window.location.href = '/publisher/edit-lesson';
    // };

    const handleTitleChange = (event) => {
        setTitleValue(event.target.value);
        setError('');
    };

    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Web Design Course</p>
            <div className={cx('contentContainer')}>
                <div className={cx('imageDiv')}>
                    <div className={cx('imageActionDiv')}>
                        <p className={cx('fileTitle')}>Course Image</p>
                        <input type="file" onChange={handleChange} />
                    </div>
                    {image && <img className={cx('imageContainer')} src={image} alt={'course'}></img>}
                </div>
                <div className={cx('courseInfoDiv')}>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Title</p>
                        <textarea
                            className={cx('input')}
                            value={titleValue}
                            placeholder={'Title name...'}
                            onChange={handleTitleChange}
                        ></textarea>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Description</p>
                        <textarea className={cx('input')} type={'text'} placeholder={'Description...'}></textarea>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Tier</p>
                        <div className={cx('courseTierDiv')}>
                            <p
                                className={cx('courseTier')}
                                style={{ opacity: isSelected === 1 ? 1 : 0.3 }}
                                onClick={() => handleClick(1)}
                            >
                                Free
                            </p>
                            <p
                                className={cx('courseTier')}
                                style={{ opacity: isSelected === 2 ? 1 : 0.3 }}
                                onClick={() => handleClick(2)}
                            >
                                Premium
                            </p>
                        </div>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>About</p>
                        <textarea
                            className={cx('inputAbout')}
                            type={'text'}
                            placeholder={'Write something...'}
                        ></textarea>
                    </div>
                </div>

                <div className={cx('lessonsContainer')}>
                    <div className={cx('topContainer')}>
                        <p className={cx('topTitle')}>Lessons</p>
                        <button className={cx('topTitleBtn')}>
                            <Link className={cx('addLessonLink')} to="add-lesson">
                                Add Lesson
                            </Link>
                        </button>
                    </div>
                    <LessonsMaterialLists lessonsList={lessonsList} />
                </div>
            </div>
            <CancelConfirmBtns />
        </div>
    );
}

export default AddCourses;
