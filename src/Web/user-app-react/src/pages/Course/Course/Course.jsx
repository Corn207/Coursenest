import { useParams } from "react-router";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";
import styles from "./Course.module.scss";
import Badge from 'react-bootstrap/Badge';
import { useNavigate } from "react-router-dom";
import config from '~/config';
import axios from 'axios';
import { Button } from "react-bootstrap";
import getNumberOfDays from "~/helper/getNumberOfDays";

export default function Course(props) {
    const { logged } = props;
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [courseInfo, setCourseInfo] = useState({});
    const [lessonInfo, setLessonInfo] = useState([]);
    // const [listLessons, setListLessons] = useState([]);
    const tokenStr = localStorage.getItem('accessToken');

    useEffect(() => {
        setIsLoading(true);
        axios
            .get(`${config.baseUrl}/api/courses/${id}`)
            .then((res) => {
                setCourseInfo(res.data);
                return axios.get(`${config.baseUrl}/api/lessons?courseId=${id}`)
            })
            .then((res) => {
                setLessonInfo(res.data);
                // return res.data;
            })
            // .then(async (listLessons) => {
            //     const newListLessons = [];
            //     await Promise.all(
            //         (listLessons).map(async (lesson) => {
            //             const response = await axios.get(`${config.baseUrl}/api/units?lessonId=${lesson.lessonId}`);
            //             const units = response.data;
            //             const newLesson = { ...lesson, "units": units }
            //             newListLessons.push(newLesson);
            //         }),
            //     );
            //     setListLessons(newListLessons);
            // })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, []);

    // const [open, setOpen] = useState(0);
    // const handleToggleButton = (lesson) => {
    //     setOpen(lesson.lessonId)
    // }

    const navigate = useNavigate();
    const handleSeeCourseDetail = async () => {
        if (!logged) {
            if (window.confirm("You need to login to see this course!")) {
                navigate(`/sign-in`);
            }
        }
        else {
            // nếu course free thì 
            // check enroll chưa, rồi thì tới trang course enroll đó
            // chưa thì hỏi enroll rồi tới trang course enroll đó
            if (courseInfo.tier == 0) {
                const enrolled = await axios.get(`${config.baseUrl}/api/enrollments`, {
                    headers: { Authorization: `Bearer ${tokenStr}` }
                });
                if ((enrolled.data).find(item => item.courseId == id)) {
                    if (window.confirm("Go to enrolled course ?")) {
                        navigate(`/enrolled/course`);
                    }
                }
                else {
                    if (window.confirm("Enroll this course ?")) {
                        if (window.confirm("Enroll this course ?")) {
                            axios
                                .post(`${config.baseUrl}/api/enrollments`, id, {
                                    headers: {
                                        'Authorization': `Bearer ${tokenStr}`,
                                        'Content-Type': 'application/json'
                                    }
                                })
                                .then(() => {
                                    navigate(`/enrolled/course`);
                                })
                                .catch((err) => {
                                    console.log(err);
                                })
                        }
                    }
                }
            }
            // nếu course premium thì 
            // check role xem phải student còn hạn ko, ko phải tới trang payment
            // nếu student còn hạn thì check enroll chưa, rồi thì tới trang course enroll đó
            // chưa thì hỏi enroll rồi tới trang course enroll đó
            else if (courseInfo.tier == 1) {
                const getRoles = await axios.get(`${config.baseUrl}/api/roles/me`, {
                    headers: {
                        Authorization: `Bearer ${tokenStr}`
                    }
                });
                getRoles.data.find(async item => {
                    if (item.type == 0 && getNumberOfDays(item.expiry) > 0) {
                        const enrolled = await axios.get(`${config.baseUrl}/api/enrollments`, {
                            headers: { Authorization: `Bearer ${tokenStr}` }
                        });
                        if ((enrolled.data).find(item => item.courseId == id)) {
                            if (window.confirm("Go to enrolled course ?")) {
                                navigate(`/enrolled/course`);
                            }
                        }
                        else {
                            if (window.confirm("Enroll this course ?")) {
                                axios
                                    .post(`${config.baseUrl}/api/enrollments`, id, {
                                        headers: {
                                            'Authorization': `Bearer ${tokenStr}`,
                                            'Content-Type': 'application/json'
                                        }
                                    })
                                    .then(() => {
                                        navigate(`/enrolled/course`);
                                    })
                                    .catch((err) => {
                                        console.log(err);
                                    })
                            }
                        }
                    }
                    else {
                        alert("Go to payment");
                        //TODO go to payment screen (Tuấn)
                    }
                })
            }
        }
    }

    if (isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.sideBar}>
                <div style={{ display: "flex", gap: 20, marginBottom: 40, alignItems: "flex-start" }}>
                    <h5>{courseInfo.title}</h5>
                    <Badge style={{ padding: 8 }} bg="success">
                        {courseInfo.tier === 0 ? 'Free' : courseInfo.tier === 1 ? 'Premium' : ''}
                    </Badge>
                </div>
                <div>
                    {lessonInfo && lessonInfo.map((lesson) => {
                        return (
                            <>
                                <div key={lesson.lessonId} style={{ marginBottom: 20 }}>
                                    <div
                                        style={{ cursor: "pointer", display: "flex", alignItems: "center", justifyContent: "space-between" }}
                                    // onClick={() => { handleToggleButton(lesson) }}
                                    >
                                        <h6>{lesson.title}</h6>
                                        {/* <img src={images.dropDownIcon} alt="" style={{ width: 20, height: 10 }} /> */}
                                    </div>
                                    {/* {open == lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                        <div key={unit.unitId} style={{ paddingLeft: 20, marginTop: 12 }}>
                                            <p onClick={() => handleOpenDetailUnit(unit)} style={{ cursor: "pointer" }}>{unit.title}</p>
                                        </div>
                                    ))} */}
                                </div>
                            </>
                        )
                    })}
                </div>
                <div>
                    <Button onClick={handleSeeCourseDetail}>Enroll this course</Button>
                </div>
            </div>
            <div className={styles.courseInfo}>
                <div>
                    <h3>{courseInfo.title}</h3>
                    <h5>{courseInfo.description}</h5>
                    <p>{courseInfo.about}</p>
                    {/* <div style={{ marginTop: 20 }}>
                        <div>
                            {listLessons && listLessons.map((lesson) => {
                                return (
                                    <>
                                        <div key={lesson.lessonId} style={{ marginBottom: 20 }}>
                                            <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
                                                <h6>{lesson.title}</h6>
                                            </div>
                                            {lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                                <div key={unit.unitId} style={{ paddingLeft: 20, marginTop: 12 }}>
                                                    <p onClick={() => handleOpenDetailUnit(unit)}
                                                        style={{ cursor: "pointer" }}>
                                                        <strong>{unit.title}</strong> ( {unit.requiredMinutes} mins )
                                                    </p>
                                                </div>
                                            ))}
                                        </div>
                                    </>
                                )
                            })}
                        </div>
                    </div> */}
                </div>
            </div>
        </div>
    );
}
