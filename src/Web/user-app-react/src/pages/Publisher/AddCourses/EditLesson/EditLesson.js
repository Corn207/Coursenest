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
                    <FontAwesomeIcon icon={faChevronRight} />
                </div>
                <p className={cx('lessonTitle')}>Lesson 1</p>
            </div>
        </div>
    );
}

export default EditLesson;
