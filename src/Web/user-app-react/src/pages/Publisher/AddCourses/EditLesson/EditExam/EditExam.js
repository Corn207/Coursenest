import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './EditExam.module.scss';

const cx = classNames.bind(styles);

function EditExam() {
    const [examTitle, setExamTitle] = useState('Title');
    const [examDesc, setExamDesc] = useState('Description');
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [isEditingDesc, setIsEditingDesc] = useState(false);
    const [isEditingTime, setIsEditingTime] = useState(false);
    const [timeLimit, setTimeLimit] = useState(45);

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setExamTitle(event.target.value);
        // setMaterial({ ...material, Title: event.target.value });
        console.log(event.target.value);
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleDescClick = () => {
        setIsEditingDesc(true);
    };

    const handleDescChange = (event) => {
        setExamDesc(event.target.value);
        // setMaterial({ ...material, Description: event.target.value });
    };

    const handleDescBlur = () => {
        setIsEditingDesc(false);
    };

    const handleTimeClick = () => {
        setIsEditingTime(true);
    };

    const handleTimeChange = (event) => {
        setTimeLimit(event.target.value);
        // setMaterial({ ...material, Time: event.target.value }); <----- To-do
    };

    const handleTimeBlur = () => {
        setIsEditingTime(false);
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('topContent')}>
                {isEditingTitle ? (
                    <input
                        type="text"
                        className={cx('titleInput')}
                        value={examTitle}
                        onChange={handleTitleChange}
                        onBlur={handleTitleBlur}
                        autoFocus
                    />
                ) : (
                    <p className={cx('examTitle')} onClick={handleTitleClick}>
                        {examTitle}
                    </p>
                )}
                {isEditingTime ? (
                    <input
                        type="text"
                        className={cx('TimeInput')}
                        value={timeLimit}
                        onChange={handleTimeChange}
                        onBlur={handleTimeBlur}
                        autoFocus
                    />
                ) : (
                    <p className={cx('timeLimit')} onClick={handleTimeClick}>
                        Time: {timeLimit}:00
                    </p>
                )}
            </div>
            {isEditingDesc ? (
                <input
                    type="text"
                    className={cx('titleInput')}
                    value={examDesc}
                    onChange={handleDescChange}
                    onBlur={handleDescBlur}
                    autoFocus
                />
            ) : (
                <p className={cx('examDesc')} onClick={handleDescClick}>
                    {examDesc}
                </p>
            )}
            <div className={cx('bodyContainer')}>
                <div className={cx('topBody')}>
                    <p className={cx('bodyTitle')}>Lessons</p>
                    <button className={cx('addQuestionBtn')}>
                        {/* <Link className={cx('addLessonLink')} to="add-lesson"> */}
                        Add Question
                        {/* </Link> */}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default EditExam;
