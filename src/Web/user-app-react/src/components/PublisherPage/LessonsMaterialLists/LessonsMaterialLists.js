import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './LessonsMaterialLists.module.scss';

const cx = classNames.bind(styles);

function LessonsMaterialLists() {
    const [lessons, setLessons] = useState([
        { LessonId: 1, Title: 'Lesson 1', Description: 'Description of lesson 1', Order: 1.5 },
        { LessonId: 2, Title: 'Lesson 2', Description: 'Description of lesson 2', Order: 2.5 },
        { LessonId: 3, Title: 'Lesson 3', Description: 'Description of lesson 3', Order: 3.5 },
        { LessonId: 4, Title: 'Lesson 4', Description: 'Description of lesson 4', Order: 4.5 },
        { LessonId: 5, Title: 'Lesson 5', Description: 'Description of lesson 5', Order: 5.5 },
    ]);

    const moveItem = (LessonId, direction) => {
        const newItems = [...lessons];
        const index = newItems.findIndex((item) => item.LessonId === LessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setLessons(newItems);
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
        <ul className={cx('wrapper')}>
            {lessons.map((item, index) => (
                <li className={cx('itemDiv')} key={item.LessonId}>
                    <p className={cx('itemTitle')}>{item.Title}</p>
                    <div className={cx('itemAction')}>
                        <p className={cx('btnAction')}>Edit</p>
                        <p className={cx('btnAction')}>Delete</p>
                        <p className={cx('itemOrder')}>{index + 1}</p>
                        <div className={cx('moveBtnContainer')}>
                            {/* {lessons[lessons.indexOf(item) + 1] && (
                                <button onClick={() => moveItem(item.LessonId, 1)}>
                                    <FontAwesomeIcon icon={faChevronUp} />
                                </button>
                            )} */}
                            <button
                                className={cx('moveBtn')}
                                style={lessons[lessons.indexOf(item) - 1] ? activeBtn : disableBtn}
                                onClick={() => moveItem(item.LessonId, -1)}
                            >
                                <FontAwesomeIcon icon={faChevronUp} />
                            </button>
                            <button
                                className={cx('moveBtn')}
                                style={lessons[lessons.indexOf(item) + 1] ? activeBtn : disableBtn}
                                onClick={() => moveItem(item.LessonId, 1)}
                            >
                                <FontAwesomeIcon icon={faChevronDown} />
                            </button>

                            {/* {lessons[lessons.indexOf(item) - 1] && (
                                <button onClick={() => moveItem(item.LessonId, -1)}>
                                    <FontAwesomeIcon icon={faChevronDown} />
                                </button>
                            )} */}
                        </div>
                    </div>
                </li>
            ))}
        </ul>
    );
}

export default LessonsMaterialLists;
