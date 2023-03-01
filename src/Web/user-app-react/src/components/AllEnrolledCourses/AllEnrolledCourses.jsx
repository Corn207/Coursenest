import styles from "./AllEnrolledCourses.module.scss";
import StarRatings from 'react-star-ratings';
import ProgressBar from 'react-bootstrap/ProgressBar';
import ReviewModal from '~/components/ReviewModal/ReviewModal';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
<<<<<<< HEAD
import axios from 'axios';
import config from '~/config';
import _ from "lodash";
=======
>>>>>>> fa5343410f40e5944c158f5939516757e8564f50

export default function AllEnrolledCourses(props) {

    const { coursesEnrollments } = props;
    const navigate = useNavigate();

    const [showModalReview, setShowModalReview] = useState(false);
<<<<<<< HEAD
    const [courseNeedRate, setCourseNeedRate] = useState({});
    const [rate, setRate] = useState()
    const [isReviewed, setIsReviewed] = useState(false);
    const userId = localStorage.getItem("userId");

    const handleClickReviewCourse = async (coursesEnrollment) => {
        setCourseNeedRate(coursesEnrollment);
        await axios.get(`${config.baseUrl}/api/ratings?CourseId=${coursesEnrollment.courseId}&UserId=${userId}`)
            .then((res) => {
                if (_.isEmpty(res.data)) {
                    setIsReviewed(false);
                }
                else {
                    setIsReviewed(true);
                    setRate(res.data)
                }
            })
            .catch(err => console.log(err))
=======
    const handleClickReviewCourse = () => {
>>>>>>> fa5343410f40e5944c158f5939516757e8564f50
        setShowModalReview(true);
    };

    // sửa lại navgate tới enroll-course của userId
    const handleClickGoToCourse = (coursesEnrollment) => {
        const courseId = coursesEnrollment.courseId;
        navigate(`/courses/${courseId}`);
<<<<<<< HEAD
    };

    // học xong mới đc review nhưng api thì rv đc miễn là đã enroll nên sửa UI theo api
=======
     };
>>>>>>> fa5343410f40e5944c158f5939516757e8564f50

    return (
        <>
            <div className={styles.listCourse}>
<<<<<<< HEAD
                {coursesEnrollments &&
                    coursesEnrollments.map((coursesEnrollment) => {
                        // let button;
                        let progress = 100;
                        if (coursesEnrollment.completed == null) progress = coursesEnrollment.progress;
                        // if (coursesEnrollment.completed != null) {
                        //     button = (
                        //         <button
                        //             className={styles.button}
                        //             onClick={() => handleClickReviewCourse(coursesEnrollment)}
                        //         >
                        //             Review
                        //         </button>
                        //     );
                        // }
                        // else {
                        //     progress = coursesEnrollment.progress;
                        //     button = (
                        //         <button
                        //             className={styles.button}
                        //             onClick={() => handleClickGoToCourse(coursesEnrollment)}
                        //         >
                        //             Go
                        //         </button>
                        //     );
                        // }
=======
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

>>>>>>> fa5343410f40e5944c158f5939516757e8564f50
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
<<<<<<< HEAD
                                    {/* {button} */}
                                    <button className={styles.button} onClick={() => handleClickGoToCourse(coursesEnrollment)}>Go</button><br />
                                    <button className={styles.button} onClick={() => handleClickReviewCourse(coursesEnrollment)}>Review</button>
=======
                                    {button}
>>>>>>> fa5343410f40e5944c158f5939516757e8564f50
                                </div>
                            </div>
                        );
                    })}
            </div>
<<<<<<< HEAD
            <ReviewModal show={showModalReview} setShow={setShowModalReview} coursesEnrollment={courseNeedRate} isReviewed={isReviewed} rate={rate} />
=======
            <ReviewModal show={showModalReview} setShow={setShowModalReview} />
>>>>>>> fa5343410f40e5944c158f5939516757e8564f50
        </>
    )
}