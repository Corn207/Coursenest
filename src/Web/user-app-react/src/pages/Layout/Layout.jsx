import { Outlet } from 'react-router-dom';
import Footer from '~/components/Footer/Footer';
import Header from '~/components/Header/Header';
import styles from './Layout.module.scss';

export default function Layout () {

    return (
        <div className={styles.container}>
            <Header />
            <div className={styles.outlet} >
                <Outlet />
            </div>
            <Footer />
        </div>
    );
}