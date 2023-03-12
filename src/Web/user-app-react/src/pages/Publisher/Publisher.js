import classNames from 'classnames/bind';
import { Outlet } from 'react-router-dom';
import PublisherSideBar from '~/components/PublisherPage/PublisherSideBar';

import styles from './Publisher.module.scss';

const cx = classNames.bind(styles);

function Publisher() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('bodyContainer')}>
                <PublisherSideBar />
                <Outlet />
            </div>
        </div>
    );
}

export default Publisher;
