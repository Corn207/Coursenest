import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { Link, NavLink } from 'react-router-dom';

import styles from './PublisherSideBar.module.scss';

const cx = classNames.bind(styles);

function PublisherSideBar() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('coursesPagesContainer')}>
                <p className={cx('tabTitle')}>Edit Courses</p>
                <div className={cx('tabsDiv')}>
                    <NavLink
                        to="courses"
                        className={({ isActive }) => (isActive ? cx('activeTab') : cx('noneActiveTab'))}
                    >
                        <p className={cx('subTab')}>Dashboard</p>
                    </NavLink>
                    <NavLink
                        to="add-course"
                        className={({ isActive }) => (isActive ? cx('activeTab') : cx('noneActiveTab'))}
                    >
                        <p className={cx('subTab')}>Add Course</p>
                    </NavLink>
                    {/* <NavLink to="/" className={({ isActive }) => (isActive ? cx('activeTab') : cx('noneActiveTab'))}>
                        <p className={cx('subTab')}>Function 2</p>
                    </NavLink> */}
                </div>
            </div>
            <div className={cx('notiTabContainer')}>
                <div className={cx('topNotification')}>
                    <p className={cx('tabTitle')}>Notification</p>
                    <div className={cx('rightTopNoti')}>
                        <button className={cx('notiAmount')}>5</button>
                        <button className={cx('deleteCourseButton')}>
                            <FontAwesomeIcon className={cx('deleteCourse')} icon={faXmark} />
                        </button>
                    </div>
                </div>
                <p className={cx('messageNoti')}>You’ve got an E-mail from your Coursenest.</p>
            </div>
        </div>
    );
}

export default PublisherSideBar;
