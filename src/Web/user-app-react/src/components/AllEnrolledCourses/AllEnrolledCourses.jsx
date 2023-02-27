import styles from "./AllEnrolledCourses.module.scss";
import StarRatings from 'react-star-ratings';
import ProgressBar from 'react-bootstrap/ProgressBar';
import ReviewModal from '~/components/ReviewModal/ReviewModal';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function AllEnrolledCourses(props) {

    const { coursesEnrollments } = props;
    const navigate = useNavigate();

    const [showModalReview, setShowModalReview] = useState(false);
    const handleClickReviewCourse = () => {
        setShowModalReview(true);
    };

    // sửa lại navgate tới enroll-course của userId
    const handleClickGoToCourse = (coursesEnrollment) => {
        const courseId = coursesEnrollment.courseId;
        navigate(`/courses/${courseId}`);
     };

    return (
        <>
            <div className={styles.listCourse}>
                {console.log(coursesEnrollments)}
                {coursesEnrollments &&
                    coursesEnrollments.map((coursesEnrollment) => {                        
                        let button;
                        let progress = 100;
                        if (coursesEnrollment.completed != null) {
                            button = (
                                <button
                                    className={styles.button}
                                    onClick={() => handleClickReviewCourse(coursesEnrollment)}
                                >
                                    Review
                                </button>
                            );
                        } 
                        else {
                            progress = coursesEnrollment.progress;
                            button = (
                                <button
                                    className={styles.button}
                                    onClick={() => handleClickGoToCourse(coursesEnrollment)}
                                >
                                    Go
                                </button>
                            );
                        }

                        return (
                            <div className={styles.course} key={coursesEnrollment.enrollmentId}>
                                <div>
                                    <img
                                        className={styles.imageCourse}
                                        src={coursesEnrollment?.cover?.uri == null ? '' : coursesEnrollment.cover.uri}
                                        alt=""
                                    />
                                </div>
                                <div className={`styles.child ${styles.childThree}`}>
                                    <h4>{coursesEnrollment.title}</h4>
                                    <p>{coursesEnrollment.description}</p>
                                    <ProgressBar className={styles.progressBar} now={progress} label={`${progress}%`} />
                                    <div className={styles.rating}>
                                        <span style={{ color: '#FFC069' }}>{coursesEnrollment.ratingAverage}</span>
                                        <span>
                                            <StarRatings
                                                starRatedColor="#FFC069"
                                                rating={coursesEnrollment.ratingAverage}
                                                starDimension="20px"
                                                starSpacing="4px"
                                                numberOfStars={5}
                                            />
                                        </span>
                                        <span>({coursesEnrollment.ratingTotal})</span>
                                    </div>
                                </div>
                                <div>
                                    {button}
                                </div>
                            </div>
                        );
                    })}
            </div>
            <ReviewModal show={showModalReview} setShow={setShowModalReview} />
        </>
    )
}