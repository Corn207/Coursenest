import { faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';

import LessonsMaterialLists from '~/components/PublisherPage/LessonsMaterialLists';

import styles from './EditLesson.module.scss';

const cx = classNames.bind(styles);

function EditLesson() {
    // const [lessonsList, setLessonsList] = useState([]);

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
                            <button className={cx('unitsBtns')}>
                                <Link className={cx('unitsAddBtns')} to={'edit-material'}>
                                    Add Material
                                </Link>
                            </button>
                            <button className={cx('unitsBtns')}>
                                <Link className={cx('unitsAddBtns')}>Add Exam</Link>
                            </button>
                        </div>
                    </div>
                    <LessonsMaterialLists lessonsList={[]} />
                </div>
            </div>
            <CancelConfirmBtns />
        </div>
    );
}

export default EditLesson;
