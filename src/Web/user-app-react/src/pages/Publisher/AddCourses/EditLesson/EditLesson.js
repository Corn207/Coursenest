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

    const lessonsList = [
        { LessonId: 1, Title: 'Lesson 1', Description: 'Description of lesson 1', Order: 1.5 },
        { LessonId: 2, Title: 'Lesson 2', Description: 'Description of lesson 2', Order: 2.5 },
        { LessonId: 3, Title: 'Lesson 3', Description: 'Description of lesson 3', Order: 3.5 },
        { LessonId: 4, Title: 'Lesson 4', Description: 'Description of lesson 4', Order: 4.5 },
        { LessonId: 5, Title: 'Lesson 5', Description: 'Description of lesson 5', Order: 5.5 },
    ];

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
                    <LessonsMaterialLists lessonsList={lessonsList} />
                </div>
            </div>
            <CancelConfirmBtns />
        </div>
    );
}

export default EditLesson;
