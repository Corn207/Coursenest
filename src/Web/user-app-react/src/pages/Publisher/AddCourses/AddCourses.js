import classNames from 'classnames/bind';
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import LessonsMaterialLists from '~/components/PublisherPage/LessonsMaterialLists';
import CourseContext from '~/contexts/courseContext';
import EditLesson from './EditLesson';

import styles from './AddCourses.module.scss';
import EditMaterial from './EditLesson/EditMaterial';
import TopicsSearch from '~/components/TopicsSearch';
import axios from 'axios';
import config from '~/config';

const cx = classNames.bind(styles);

function AddCourses({ isEditCourse }) {
    const [image, setImage] = useState(null);
    const [tier, setTier] = useState(0);
    const [titleValue, setTitleValue] = useState('');
    const [descriptionValue, setDescriptionValue] = useState('');
    const [aboutValue, setAboutValue] = useState('');
    const [lessons, setLessons] = useState([]);
    const [step, setStep] = useState(0);
    const [lessonTitle, setLessonTitle] = useState('');
    const [chosenLesson, setChosenLesson] = useState(null);
    const [interestedTopicId, setinterestedTopicIdId] = useState(null);

    let params = useParams();
    const navigate = useNavigate();

    const handleTitleValue = (value) => {
        setLessonTitle(value);
    };

    const handleUpdateListOnAddClick = (list) => {
        setLessons(list);
    };

    const handleLessonUpdate = (updatedLesson) => {
        const updatedLessons = lessons.map((lesson) => {
            if (lesson.LessonId === updatedLesson.LessonId) {
                return updatedLesson;
            } else {
                return lesson;
            }
        });
        setLessons(updatedLessons);
        console.log(updatedLessons);
    };

    const handleOnEdit = (choseLesson) => {
        setChosenLesson(choseLesson);
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
        setTier(id);
    };

    const handleTitleChange = (event) => {
        setTitleValue(event.target.value);
    };

    const handleDescChange = (event) => {
        setDescriptionValue(event.target.value);
    };

    const handleAboutChange = (event) => {
        setAboutValue(event.target.value);
    };

    const handleCancel = () => {
        navigate(`/publisher/${params.PublisherUserId}`);
    };

    const onSubmit = (event) => {
        event.preventDefault();
        console.log('done onSubmit');
    };

    const handleSubmit = async () => {
        if (!titleValue.trim()) {
            alert('Please enter a course name!!');
            return;
        } else if (interestedTopicId === null) {
            alert('Please chose your topic!!');
            return;
        } else {
            try {
                const res = await axios.post(`${config.baseUrl}/api/courses`, {
                    title: titleValue,
                    description: descriptionValue,
                    about: aboutValue,
                    tier: tier,
                    topicId: interestedTopicId,
                });
                // console.log({
                //     title: titleValue,
                //     description: descriptionValue,
                //     about: aboutValue,
                //     tier: tier,
                //     topicId: interestedTopicId,
                // });
                console.log('da~ them course: ' + res.data);
            } catch (error) {
                console.log(error);
            }
        }

        console.log('done');
    };

    const handleGetTopics = (topicData) => {
        setinterestedTopicIdId(topicData);
    };

    return (
        <div className={cx('divWrapper')}>
            {step === 0 && (
                <form className={cx('formWrapper')} onSubmit={onSubmit}>
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
                                            style={{ opacity: tier === 0 ? 1 : 0.3 }}
                                            onClick={() => handleClick(0)}
                                        >
                                            Free
                                        </p>
                                        <p
                                            className={cx('courseTierPremium')}
                                            style={{ opacity: tier === 1 ? 1 : 0.3 }}
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
                                <TopicsSearch handleGetTopics={handleGetTopics} maxTopics={1} />
                            </div>

                            {isEditCourse && (
                                <LessonsMaterialLists
                                    lessonsList={lessons}
                                    // editedTitleValue={lessonEditedTitle}
                                    getLessonsListOnAdd={handleUpdateListOnAddClick}
                                    onEdit={handleOnEdit}
                                    handleNextStep={handleNextStep}
                                    handleTitleValue={handleTitleValue}
                                />
                            )}
                        </div>
                        <CancelConfirmBtns onCancel={handleCancel} onConfirm={handleSubmit} />
                    </div>
                </form>
            )}
            {step === 1 && (
                <EditLesson
                    chosenLesson={chosenLesson}
                    // lessonsList={lessons}
                    titleValue={titleValue}
                    lessonTitle={lessonTitle}
                    handleLessonUpdate={handleLessonUpdate}
                    handleNextStep={handleNextStep}
                    handleBackStep={handleBackStep}
                    // onConfirmClick={handleTitleEditedValue}
                />
            )}
        </div>
    );
}

export default AddCourses;
