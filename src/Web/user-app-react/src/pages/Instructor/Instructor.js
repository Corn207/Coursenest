import { Outlet } from 'react-router-dom';
import classNames from 'classnames/bind';

import Footer from '~/components/Footer/Footer';
import Header from '~/components/Header/Header';
import InstructorSideBar from '~/components/InstructorSideBar';

import styles from './Instructor.module.scss';

const cx = classNames.bind(styles);

function Instructor() {
    return (
        <div className={cx('wrapper')}>
            <Header />
            <div className={cx('bodyWrapper')}>
                <InstructorSideBar />
                <Outlet />
            </div>
            <Footer />
        </div>
    );
}

export default Instructor;
