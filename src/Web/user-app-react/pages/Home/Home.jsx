import CoursesInProgress from '~/components/CoursesInProgress/CoursesInProgress';
import styles from './Home.module.scss';
import { useState } from 'react';

export default function Home() {

    const [authenticated, setauthenticated] = useState(true);

    return (
        <div className={styles.container}>
            <div className={styles.slogan}>
                <h1 className={styles.title}># ONLINE COURSE EXAM</h1>
                <span className={styles.subTitle}>BY TEAM 3</span>
            </div>

            {authenticated && (
                <CoursesInProgress />
            )}

        </div>
    );
}