import { faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';

import styles from './EditLesson.module.scss';

const cx = classNames.bind(styles);

function EditLesson() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('topTitle')}>
                <p className={cx('courseTitle')}>Title Course </p>
                <div>
                    <FontAwesomeIcon className={cx('icon')} icon={faChevronRight} />
                </div>
                <p className={cx('lessonTitle')}>Lesson 1</p>
            </div>
            <div className={cx('lessonBody')}>
                <p className={cx('lessonTitleDetail')}>{'Lesson 1: How to...'} </p>
                <p className={cx('lessonDesc')}>{'This will teach you...'} </p>
                <div className={cx('unitsBody')}>
                    <div>
                        <p>Units</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default EditLesson;
