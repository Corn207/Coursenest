import classNames from 'classnames/bind';
import { Outlet } from 'react-router-dom';
import Header from '~/components/Header/Header';
import PublisherSideBar from '~/components/PublisherPage/PublisherSideBar';

import styles from './Publisher.module.scss';

const cx = classNames.bind(styles);

function Publisher() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('headerContainer')}>
                <Header />
            </div>
            <div className={cx('bodyContainer')}>
                <PublisherSideBar />
                <Outlet />
            </div>
        </div>
    );
}

export default Publisher;
