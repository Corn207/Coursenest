import classNames from 'classnames/bind';
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import LessonsMaterialLists from '~/components/PublisherPage/LessonsMaterialLists';
import CourseContext from '~/contexts/courseContext';
import EditLesson from './EditLesson';

import styles from './AddCourses.module.scss';
import EditMaterial from './EditLesson/EditMaterial';

const cx = classNames.bind(styles);

function AddCourses() {
    const [image, setImage] = useState(null);
    const [isSelected, setIsSlected] = useState(0);
    const [titleValue, setTitleValue] = useState('');
    const [descriptionValue, setDescriptionValue] = useState('');
    const [aboutValue, setAboutValue] = useState('');
    const [error, setError] = useState('');
    const [courseData, setCourseData] = useState({});
    const [lessons, setLessons] = useState([]);
    const [step, setStep] = useState(0);
    const [lessonTitle, setLessonTitle] = useState('');
    const [lessonEditedTitle, setLessonEditedTitle] = useState('');
    const [chosenLesson, setChosenLesson] = useState(null);

    let params = useParams();
    const navigate = useNavigate();

    // const lessonsList = [
    //     { LessonId: 1, Title: 'Lesson 1', Description: 'Description of lesson 1', Order: 1.5 },
    //     { LessonId: 2, Title: 'Lesson 2', Description: 'Description of lesson 2', Order: 2.5 },
    //     { LessonId: 3, Title: 'Lesson 3', Description: 'Description of lesson 3', Order: 3.5 },
    //     { LessonId: 4, Title: 'Lesson 4', Description: 'Description of lesson 4', Order: 4.5 },
    //     { LessonId: 5, Title: 'Lesson 5', Description: 'Description of lesson 5', Order: 5.5 },
    // ];
    const handleTitleValue = (value) => {
        setLessonTitle(value);
    };

    const handleTitleEditedValue = (value) => {
        setLessonEditedTitle(value);
    };

    const handleEditLesson = (lessonObj) => {
        setChosenLesson(lessonObj);
    };

    const handleNextStep = () => {
        setStep(step + 1);
    };

    const handleBackStep = () => {
        setStep(step - 1);
    };

    const handleChange = (e) => {
        setImage(URL.createObjectURL(e.target.files[0]));
    };

    const handleClick = (id) => {
        setIsSlected(id);
    };

    const handleTitleChange = (event) => {
        setTitleValue(event.target.value);
        setError('');
    };

    const handleDescChange = (event) => {
        setDescriptionValue(event.target.value);
        setError('');
    };

    const handleAboutChange = (event) => {
        setAboutValue(event.target.value);
        setError('');
    };

    const handleCancel = () => {
        navigate(`/publisher/${params.PublisherUserId}`);
    };

    function handleSubmit(event) {
        event.preventDefault();
        navigate('/publisher/add-course/add-lesson');
    }

    return (
        <CourseContext.Provider value={{ courseData, setCourseData, lessons, setLessons }}>
            {step === 0 && (
                <form className={cx('formWrapper')} onSubmit={handleSubmit}>
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
                                    <textarea
                                        className={cx('input')}
                                        value={descriptionValue}
                                        type={'text'}
                                        placeholder={'Description...'}
                                        onChange={handleDescChange}
                                    ></textarea>
                                </div>
                                <div className={cx('courseInputInfo')}>
                                    <p className={cx('inputTitle')}>Course Tier</p>
                                    <div className={cx('courseTierDiv')}>
                                        <p
                                            className={cx('courseTier')}
                                            style={{ opacity: isSelected === 0 ? 1 : 0.3 }}
                                            onClick={() => handleClick(0)}
                                        >
                                            Free
                                        </p>
                                        <p
                                            className={cx('courseTierPremium')}
                                            style={{ opacity: isSelected === 1 ? 1 : 0.3 }}
                                            onClick={() => handleClick(1)}
                                        >
                                            Premium
                                        </p>
                                    </div>
                                </div>
                                <div className={cx('courseInputInfo')}>
                                    <p className={cx('inputTitle')}>About</p>
                                    <textarea
                                        className={cx('inputAbout')}
                                        value={aboutValue}
                                        type={'text'}
                                        placeholder={'Write something...'}
                                        onChange={handleAboutChange}
                                    ></textarea>
                                </div>
                            </div>

                            <LessonsMaterialLists
                                lessonsList={lessons}
                                editedTitleValue={lessonEditedTitle}
                                onEdit={handleEditLesson}
                                handleNextStep={handleNextStep}
                                handleTitleValue={handleTitleValue}
                            />
                        </div>
                        <CancelConfirmBtns onCancel={handleCancel} />
                    </div>
                </form>
            )}
            {step === 1 && (
                <EditLesson
                    titleValue={titleValue}
                    lessonTitle={lessonTitle}
                    chosingLesson={chosenLesson}
                    handleNextStep={handleNextStep}
                    handleBackStep={handleBackStep}
                    onConfirmClick={handleTitleEditedValue}
                />
            )}
        </CourseContext.Provider>
    );
}

export default AddCourses;
