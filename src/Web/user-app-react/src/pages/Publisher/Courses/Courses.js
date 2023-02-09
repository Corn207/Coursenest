import classNames from 'classnames/bind';
import PublisherSideBar from '~/components/PublisherPage/PublisherSideBar';

import styles from './Courses.module.scss';

const cx = classNames.bind(styles);

function Courses() {
    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Courses</p>
            <div className={cx('courses-dashboard')}>
                <div className={cx('mainRow')}>
                    <input type={'checkbox'}></input>
                    <p>Title</p>
                    <p>Description</p>
                    <p>Status</p>
                    <p>Topic</p>
                    <p>Course Tier</p>
                </div>
                <div>Courses</div>
            </div>
        </div>
    );
}

export default Courses;
