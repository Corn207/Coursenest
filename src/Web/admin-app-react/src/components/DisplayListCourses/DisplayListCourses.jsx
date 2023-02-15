import styles from "./DisplayListCourses.module.css";
import "font-awesome/css/font-awesome.min.css";
import courseAvatar from "../../assets/courseAvatarDefault.png"

function DisplayListCourses(props) {

    const {listCourses, handleClickUpdateCourse} = props;
   
    return(
        <div>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th scope="col">Title</th>
                        <th scope="col">Description</th>
                        <th scope="col">Status</th>
                        <th scope="col">Topic</th>
                        <th scope="col">Course Tier</th>
                        <th scope="col">Actions</th>
                    </tr>
                </thead>
                <tbody>
                   { listCourses && listCourses.map((course) => {
                        return (
                            <tr className={styles.tableRow} key={course.courseId}>
                                <td className={styles.avatarWrapper}>
                                    <img src={course.cover?.uri ?? courseAvatar} className={styles.avatar} alt=""/>
                                    <div>
                                        <p>{course.title}</p>
                                    </div>
                                </td>
                                <td>
                                    <p>{course.description}</p>
                                </td>
                                <td className={styles.paddingTop}>
                                    <p>{course.isApproved == true ? "Approved" : "Pending"}</p>
                                </td>
                                <td className={styles.paddingTop}>
                                    <p>{course.topicTitle}</p>
                                </td>
                                <td className={styles.paddingTop}>
                                    <p>{course.tier == 0 ? "Free" : "Premium"}</p>
                                </td>
                                <td>
                                    <button
                                        className={`btn btn-secondary btn-sm ${styles.action}`}
                                        onClick={handleClickUpdateCourse(course)}
                                    >
                                        <i className="fa fa-edit"></i>
                                    </button>
                                </td>
                            </tr>
                        )
                   })}
                </tbody>
            </table>
        </div>
    );
}

export default DisplayListCourses;