import { useParams } from "react-router";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import config from "~/config";
import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function Units(props) {
    const { logged } = props;
    // đã login, check enroll (gọi qua api get all enroll), chưa enrol thì show hỏi enroll
    // cần check payment nếu course đó premium

    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [courseInfo, setCourseInfo] = useState({});
    const [listLessons, setListLessons] = useState([])
    const navigate = useNavigate()

    useEffect(() => {
       setIsLoading(true);
       axios
            .get(`${config.baseUrl}/api/courses/${id}`)
            .then((res) => {
                setCourseInfo(res.data);
                return axios.get(`${config.baseUrl}/api/lessons?courseId=${id}`)
            })
            .then((res) => {
                return res.data;
            })
            .then(async (listLessons) => {
                const newListLessons = [];
                await Promise.all(
                    (listLessons).map(async (lesson) => {
                        const response = await axios.get(`${config.baseUrl}/api/units?lessonId=${lesson.lessonId}`);
                        const units = response.data;
                        const newLesson = {...lesson, "units": units}
                        newListLessons.push(newLesson);
                    }),
                );
                setListLessons(newListLessons);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, []);

    const handleOpenDetailUnit = (unit) => {
        if (!logged) {
            if (window.confirm("You need to login to see this course!")) {
                navigate(`/sign-in`);
            }
        }
        else {
            // check course free
            if(courseInfo.tier == 0) {

            }
            else if (courseInfo.tier == 1) {
                
            }
        }
    }
 
    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{courseInfo.title}</h3>
            <h5>{courseInfo.description}</h5>
            <p>{courseInfo.about}</p>
            <div style={{marginTop: 20}}>
            <div>
                {listLessons && listLessons.map((lesson) => {
                    return (
                        <>
                            <div key={lesson.lessonId} style={{marginBottom: 20}}>
                                <div style={{display: "flex", alignItems: "center", justifyContent: "space-between"}}>
                                    <h6>{lesson.title}</h6>
                                </div>
                                {lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                    <div key={unit.unitId} style={{paddingLeft: 20, marginTop: 12}}>
                                        <p  onClick={() => handleOpenDetailUnit(unit)}
                                            style={{cursor: "pointer"}}>
                                            <strong>{unit.title}</strong> ( {unit.requiredMinutes} mins )
                                        </p>
                                    </div>
                                ))}
                            </div>
                        </>
                    )
                })}
                </div>
            </div>
        </div>
    )
}