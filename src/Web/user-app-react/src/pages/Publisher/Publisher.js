import classNames from 'classnames/bind';
import { Outlet } from 'react-router-dom';
import Header from '~/components/Header/Header';

import styles from './Publisher.module.scss';

const cx = classNames.bind(styles);

function Publisher() {
    return (
        <div className={cx('wrapper')}>
            <Header />
            <div className={cx('bodyContainer')}>
                <Outlet />
            </div>
        </div>
    );
}

export default Publisher;
