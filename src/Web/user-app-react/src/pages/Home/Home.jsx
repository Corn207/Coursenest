import InterestedTopics from '~/components/InterestedTopics/InterestedTopics';
import styles from './Home.module.scss';

export default function Home(props) {
    const { logged } = props;
    return (
        <div className={styles.container}>
            <div className={styles.slogan}>
                <h1 className={styles.title}># ONLINE COURSE EXAM</h1>
                <span className={styles.subTitle}>BY TEAM 3</span>
            </div>

            <div>
                {logged && <InterestedTopics />}
            </div>
        </div>
    );
}
