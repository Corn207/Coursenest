import { faChevronDown, faChevronRight, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import EditExam from './EditExam';

import styles from './EditLesson.module.scss';
import EditMaterial from './EditMaterial';

const cx = classNames.bind(styles);

function EditLesson({ chosenLesson, titleValue, lessonTitle, handleLessonUpdate, handleBackStep }) {
    const [lesson, setLesson] = useState(chosenLesson);
    const [materials, setMaterials] = useState([]);
    const [lessonEditTitle, setLessonEditTitle] = useState(lessonTitle);
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [stepLesson, setStepLesson] = useState(0);
    const [lessonDesc, setLessonDesc] = useState(chosenLesson.Description);
    const [isEditingDesc, setIsEditingDesc] = useState(false);

    const handleEditMaterialClick = () => {
        setStepLesson(1);
    };

    const handleAddExamClick = () => {
        setStepLesson(2);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setLessonEditTitle(event.target.value);
        setLesson({ ...lesson, Title: event.target.value });
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleDescClick = () => {
        setIsEditingDesc(true);
    };

    const handleDescChange = (event) => {
        setLessonDesc(event.target.value);
        setLesson({ ...lesson, Description: event.target.value });
    };

    const handleDescBlur = () => {
        setIsEditingDesc(false);
    };

    const handleCancelMaterialClick = () => {
        setStepLesson(0);
    };

    const navigate = useNavigate();
    let params = useParams();

    const moveItem = (LessonId, direction) => {
        const newItems = [...materials];
        const index = newItems.findIndex((item) => item.LessonId === LessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setMaterials(newItems);
    };

    const handleAddMaterialClick = () => {
        if (materials.length === 0) {
            const defaultNewLesson = {
                LessonId: 1,
                Title: `New item 1`,
                Description: 'Description of item ',
            };
            const addedmaterialsList = [defaultNewLesson];
            setMaterials(addedmaterialsList);
        } else {
            const defaultNewLesson = {
                LessonId: materials[materials.length - 1].LessonId + 1,
                Title: `New item ${materials[materials.length - 1].LessonId + 1}`,
                Description: 'Description of item ',
            };
            const addedmaterialsList = [...materials, defaultNewLesson];
            setMaterials(addedmaterialsList);
        }
        console.log(materials);
    };

    const handleDeleteLesson = (id) => {
        const newArrLesson = [...materials.filter((item) => item.LessonId !== id)];
        setMaterials(newArrLesson);
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
        handleLessonUpdate(lesson);
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
                        {isEditingDesc ? (
                            <input
                                type="text"
                                className={cx('titleInput')}
                                value={lessonDesc}
                                onChange={handleDescChange}
                                onBlur={handleDescBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('lessonDesc')} onClick={handleDescClick}>
                                {lessonDesc}
                            </p>
                        )}
                        <div className={cx('unitsBody')}>
                            <div className={cx('unitsTopBody')}>
                                <p className={cx('unitsTitle')}>Units</p>
                                <div className={cx('unitsBtns')}>
                                    <button className={cx('unitsBtn')} onClick={handleAddMaterialClick}>
                                        Add Material
                                    </button>
                                    <button className={cx('unitsBtn')} onClick={handleAddExamClick}>
                                        Add Exam
                                    </button>
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
            {stepLesson === 2 && <EditExam />}
        </>
    );
}

export default EditLesson;
