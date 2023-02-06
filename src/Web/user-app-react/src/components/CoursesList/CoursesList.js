import { React } from 'react';
import classNames from 'classnames/bind';

import styles from './CoursesList.module.scss';

const cx = classNames.bind(styles);

function CoursesList({ courses, children, onChose }) {
    const handleItemClick = (id) => {
        onChose(id);
    };

    return (
        <ul className={cx('course-list')}>
            {courses.map((item, index) => (
                <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.id)}>
                    {item.title}
                    {children}
                </li>
            ))}
        </ul>
    );
}

export default CoursesList;
