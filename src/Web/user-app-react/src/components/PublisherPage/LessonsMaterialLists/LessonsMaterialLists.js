import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './LessonsMaterialLists.module.scss';

const cx = classNames.bind(styles);

function LessonsMaterialLists({ lessonsList }) {
    const [lessons, setLessons] = useState(lessonsList);

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
                                onClick={() =>
                                    lessons[lessons.indexOf(item) - 1]
                                        ? moveItem(item.LessonId, -1)
                                        : console.log('not allowed to click')
                                }
                            >
                                <FontAwesomeIcon icon={faChevronUp} />
                            </button>
                            <button
                                className={cx('moveBtn')}
                                style={lessons[lessons.indexOf(item) + 1] ? activeBtn : disableBtn}
                                onClick={() =>
                                    lessons[lessons.indexOf(item) + 1]
                                        ? moveItem(item.LessonId, 1)
                                        : console.log('not allowed to click')
                                }
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
