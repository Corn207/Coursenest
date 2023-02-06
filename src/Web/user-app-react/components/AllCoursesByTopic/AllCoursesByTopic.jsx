import Badge from 'react-bootstrap/Badge';
import styles from './AllCoursesByTopic.module.scss';

export default function AllCoursesByTopic (props) {

    const { listCourses } = props;

    return (
        <div className={styles.container}>
            {
                listCourses && 
                    listCourses.map((course) => {
                        return (
                            <div className={styles.courseCard} key={course.courseId}>
                                <div>
                                    <img className={styles.imageCourse} src={course?.cover?.uri == null ? '' : course.cover.uri } alt="" />
                                </div>
                                <div className={`styles.child ${styles.childThree}`}>
                                    <h4>{course.title}</h4>
                                    <p>{course.description}</p>
                                </div>
                                <div className={styles.tier}>
                                    <Badge className={styles.badge} bg="success">{
                                        course.tier === 0 ? 'Free' : course.tier === 1 ? 'Premium' : ''
                                    }</Badge>
                                </div>
                            </div>
                        )
                    }
                )
            }
        </div>
    );
}