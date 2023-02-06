import axios from 'axios';
import AllCoursesByTopic from '~/components/AllCoursesByTopic/AllCoursesByTopic';
import { useState, useEffect } from 'react';
import 'rsuite/dist/rsuite.min.css';
import { Pagination } from 'rsuite/';
import styles from './Topic.module.scss';

export default function Topic(props) {

    const topicName = 'JavaScript'; // content

    const [listCourses, setListCourses] = useState([]);
    const [countCourse, setCountCourse] = useState(40);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(5);

    // thiếu api count course để paginate
    // vấn đề về approve courses

    useEffect(() => {
        fetchListCourses();
    }, [page, pageSize]);

    const fetchListCourses = () => {
        axios
            // .get(`http://localhost:21003/courses?TopicId=1&Page=${page - 1}&PageSize=${pageSize}`)
            .get(`http://localhost:3000/courses`)
            .then((res) => {
                setListCourses(res.data);
            })
            .catch((err) => {
                console.log(err);
            });
    };

    const handleOnChangePage = (event) => {
        setPage(parseInt(event));
    };

    return (
        <div>

            <h3>{topicName}</h3>
            <h4>All Courses</h4>
            <span>{countCourse} results</span>

            <AllCoursesByTopic listCourses={listCourses} />

            {countCourse > pageSize && (
                <div className={styles.pagination}>
                    <Pagination
                        prev
                        last
                        next
                        first
                        size="md"
                        total={countCourse}
                        limit={pageSize}
                        maxButtons={5}
                        activePage={page}
                        onChangePage={handleOnChangePage}
                    />
                </div>
            )}

        </div>
    );
}
