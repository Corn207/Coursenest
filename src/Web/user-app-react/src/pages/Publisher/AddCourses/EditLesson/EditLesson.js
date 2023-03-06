import { faChevronDown, faChevronRight, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useContext, useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import CourseContext from '~/contexts/courseContext';

import styles from './EditLesson.module.scss';
import EditMaterial from './EditMaterial';

const cx = classNames.bind(styles);

function EditLesson({
    chosenLesson,
    // materialsList,
    titleValue,
    lessonTitle,
    handleLessonUpdate,
    handleNextStep,
    handleBackStep,
    onConfirmClick,
}) {
    const { courseData, setCourseData } = useContext(CourseContext);

    const [lesson, setLesson] = useState(chosenLesson);
    const [materials, setmaterials] = useState([]);
    const [lessonEditTitle, setLessonEditTitle] = useState(lessonTitle);
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [stepLesson, setStepLesson] = useState(0);

    // useEffect(() => {
    //     setLessonEditTitle(lessonTitle);
    // }, [lessonTitle]);

    const handleEditMaterialClick = () => {
        setStepLesson(stepLesson + 1);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setLessonEditTitle(event.target.value);
        setLesson({ ...lesson, Title: event.target.value });
    };

    // const handleUpdateLesson = (updatedItem) => {
    //     // Find the lesson object with the corresponding ID
    //     const lessonToUpdate = materials.find((lesson) => lesson.LessonId === updatedItem.LessonId);
    //     // Create a new lesson object with the updated title
    //     const updatedLesson = { ...lessonToUpdate, title: lessonEditTitle };
    //     // Pass the updated lesson back to the parent component
    //     onLessonUpdate(updatedLesson);
    // };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleCancelMaterialClick = () => {
        setStepLesson(stepLesson - 1);
    };

    const navigate = useNavigate();
    let params = useParams();

    // const [materialsList, setmaterialsList] = useState([]);

    const moveItem = (LessonId, direction) => {
        const newItems = [...materials];
        const index = newItems.findIndex((item) => item.LessonId === LessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setmaterials(newItems);
    };

    const handleAddLessonClick = () => {
        if (materials.length === 0) {
            const defaultNewLesson = {
                LessonId: 1,
                Title: `New item 1`,
                Description: 'Description of item ',
            };
            const addedmaterialsList = [defaultNewLesson];
            setmaterials(addedmaterialsList);
        } else {
            const defaultNewLesson = {
                LessonId: materials[materials.length - 1].LessonId + 1,
                Title: `New item ${materials[materials.length - 1].LessonId + 1}`,
                Description: 'Description of item ',
            };
            const addedmaterialsList = [...materials, defaultNewLesson];
            setmaterials(addedmaterialsList);
        }
        // setmaterials(addedmaterialsList);
        console.log(materials);
    };

    const handleDeleteLesson = (id) => {
        const newArrLesson = [...materials.filter((item) => item.LessonId !== id)];
        setmaterials(newArrLesson);
        console.log(newArrLesson);
    };

    const handleEditLesson = (id) => {
        handleEditMaterialClick();
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson/edit-material`);
    };

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course`);
    };

    const handleConfirm = () => {
        handleBackStep();
        // chosingLesson.Title = { lessonEditTitle };
        // const newLesson = {Title: lessonEditTitle,...chosingLesson}
        // onConfirmClick(lessonEditTitle);
        handleLessonUpdate(lesson);
        console.log(lesson);
        // navigate(`/publisher/${params.PublisherUserId}/add-course`);
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
        <>
            {stepLesson === 0 && (
                <div className={cx('wrapper')}>
                    <div className={cx('topTitle')}>
                        <p className={cx('courseTitle')}>{titleValue}</p>
                        <div>
                            <FontAwesomeIcon className={cx('icon')} icon={faChevronRight} />
                        </div>
                        <p className={cx('lessonTitle')}>{lessonEditTitle}</p>
                    </div>
                    <div className={cx('lessonBody')}>
                        {isEditingTitle ? (
                            <input
                                type="text"
                                className={cx('titleInput')}
                                value={lessonEditTitle}
                                onChange={handleTitleChange}
                                onBlur={handleTitleBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('lessonTitleDetail')} onClick={handleTitleClick}>
                                {'Title: ' + lessonEditTitle}
                            </p>
                        )}
                        {/* <p className={cx('lessonDesc')}>{'This will teach you...'} </p> */}
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
                                {materials.map((item, index) => (
                                    <li className={cx('itemDiv')} key={item.LessonId}>
                                        <p className={cx('itemTitle')}>{item.Title}</p>
                                        <div className={cx('itemAction')}>
                                            <p className={cx('btnAction')} onClick={handleEditLesson}>
                                                Edit
                                            </p>
                                            <p
                                                className={cx('btnAction')}
                                                onClick={() => handleDeleteLesson(item.LessonId)}
                                            >
                                                Delete
                                            </p>
                                            <p className={cx('itemOrder')}>{index + 1}</p>
                                            <div className={cx('moveBtnContainer')}>
                                                <button
                                                    className={cx('moveBtn')}
                                                    style={
                                                        materials[materials.indexOf(item) - 1] ? activeBtn : disableBtn
                                                    }
                                                    onClick={() =>
                                                        materials[materials.indexOf(item) - 1]
                                                            ? moveItem(item.LessonId, -1)
                                                            : console.log('not allowed to click')
                                                    }
                                                >
                                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronUp} />
                                                </button>
                                                <button
                                                    className={cx('moveBtn')}
                                                    style={
                                                        materials[materials.indexOf(item) + 1] ? activeBtn : disableBtn
                                                    }
                                                    onClick={() =>
                                                        materials[materials.indexOf(item) + 1]
                                                            ? moveItem(item.LessonId, 1)
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
                    </div>
                    <CancelConfirmBtns onConfirm={handleConfirm} onCancel={handleCancel} />
                </div>
            )}
            {stepLesson === 1 && <EditMaterial handleBackStep={handleCancelMaterialClick} />}
        </>
    );
}

export default EditLesson;
