import classNames from 'classnames/bind';

import styles from './EditExam.module.scss';

const cx = classNames.bind(styles);

function EditExam() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('topContent')}>
                <p className={cx('examTitle')}>Title</p>
                <p className={cx('timeLimit')}>Time: 45:00</p>
            </div>
            <p className={cx('examDesc')}>Desc</p>
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
