import { useState, useEffect } from 'react';
import axios from 'axios';
import styles from './MyCourses.module.scss';
import StarRatings from 'react-star-ratings';
import ProgressBar from 'react-bootstrap/ProgressBar';
import ReviewModal from '~/components/ReviewModal/ReviewModal';

export default function MyCourses() {
    // userId : 19

    const tokenStr = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE5IiwiZXhwIjoxNjc1ODgwODkzfQ.02g_GZMLww2i3y-pjbWRhqnmCXqJrVPGNexPv6fFDMg`;

    const [coursesEnrollments, setCoursesEnrollments] = useState([]);
    const [inProgressCourses, setInProgressCourses] = useState([]);
    const [completedCourses, setCompletedCourses] = useState([]);
    const [showModalReview, setShowModalReview] = useState(false);

    useEffect(() => {
        axios
            .get(`http://coursenest.corn207.loseyourip.com/enrollments`, {
                headers: { Authorization: `Bearer ${tokenStr}` },
            })
            .then((res) => {
                console.log(res.data);
                return res.data;
            })
            .then(async (enrollments) => {
                const allCoursesEnrollments = [];
                await Promise.all(
                    enrollments.map(async (enrollment) => {
                        const response = await axios.get(
                            `http://coursenest.corn207.loseyourip.com/enrollments/${enrollment.courseId}`,
                            { headers: { Authorization: `Bearer ${tokenStr}` } },
                        );
                        const course = response.data;
                        const coursesEnrollment = { ...enrollment, ...course };
                        allCoursesEnrollments.push(coursesEnrollment);
                    }),
                );
                setCoursesEnrollments(allCoursesEnrollments);
            })
            .catch((err) => console.log(err));
    }, []);

    const handleClickReviewCourse = () => {
        setShowModalReview(true);
    };

    const handleClickGoToCourse = () => {};

    return (
        <div className={styles.container}>
            <div>
                <h3>My courses</h3>
            </div>
            <div>
                {/* course chưa complete thì đẩy lên trên, complete đẩy xuống dưới */}
                <h4>In-Progress Courses</h4>
                <h4>Completed Courses</h4>
            </div>

            <div className={styles.listCourse}>
                {coursesEnrollments &&
                    coursesEnrollments.map((coursesEnrollment) => {
                        let button;
                        const progress = coursesEnrollment.completed == null ? 60 : 100; // test
                        if (coursesEnrollment.completed != null) {
                            button = (
                                <button
                                    className={styles.button}
                                    onClick={() => handleClickReviewCourse(coursesEnrollment)}
                                >
                                    Review
                                </button>
                            );
                        } else
                            button = (
                                <button
                                    className={styles.button}
                                    onClick={() => handleClickGoToCourse(coursesEnrollment)}
                                >
                                    Go
                                </button>
                            );

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
                                    {/* voi moi course cua user check progress (unitsdone/totalunits) */}
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
                                {button}
                            </div>
                        );
                    })}
            </div>
            <ReviewModal show={showModalReview} setShow={setShowModalReview} />
        </div>
    );
}
