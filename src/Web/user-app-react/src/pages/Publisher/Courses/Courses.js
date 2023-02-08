import classNames from 'classnames/bind';
import PublisherSideBar from '~/components/PublisherPage/PublisherSideBar';

import styles from './Courses.module.scss';

const cx = classNames.bind(styles);

function Courses() {
    return <div className={cx('wrapper')}>Dashboard</div>;
}

export default Courses;
