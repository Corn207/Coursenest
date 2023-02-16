import React, { useState, useEffect } from "react";
import instance from '../../api/request';
import { Pagination } from "rsuite/";
import Search from "../Search";
import styles from "./ManageCourses.module.css";
import DisplayListCourses from "../DisplayListCourses/DisplayListCourses";

export default function ManageCourses() {

    const [listCourses, setListCourses] = useState([]);
    const [countCourses, setCountCourses] = useState();
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(5);
    const [keyword, setKeyWord] = useState();

    useEffect(() => {
        fetchListCourses();
    }, [page, pageSize]);

    const fetchListCourses = () => {
        instance
            .get(`/courses?PageNumber=${page}&PageSize=${pageSize}`)
            .then((res) => {
                console.log(res.data.queried);
                setListCourses(res.data.queried);
                setCountCourses(res.data.total)
            })
            .catch((err) => console.log(err));
    };

    const handleOnChangePage = (event) => {
        setPage(parseInt(event));
    };

    const handleOnChangePageSize = event => {
        console.log(event.target.value);
        setPageSize(parseInt(event.target.value));
    };

    const handleClickUpdateCourse = (course) => {
        console.log(course);
        // setShowModalUpdateUser(true);
        // setDataNeedUpdate(user);
    };

    const options = [
        { value: '5', text: '5 courses/page' },
        { value: '50', text: '50 courses/page' },
        { value: '100', text: '100 courses/page' }
    ];

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <Search setKeyWord={setKeyWord} />
                <div className={styles.courseCount}>
                    <span>Total: {countCourses} Courses</span>
                </div>
                <div>
                    <select value={pageSize} onChange={handleOnChangePageSize}>
                        {options.map(option => (
                            <option key={option.value} value={option.value}>
                                {option.text}
                            </option>
                        ))}
                    </select>
                </div>
            </div>
            <DisplayListCourses
                listCourses={listCourses}
                handleClickUpdateCourse={handleClickUpdateCourse}
            />
            <div className={styles.pagination}>
                <div className={styles.center}>
                    <Pagination
                        prev
                        last
                        next
                        first
                        size="lg"
                        total={countCourses}
                        limit={pageSize}
                        maxButtons={5}
                        activePage={page}
                        onChangePage={handleOnChangePage}
                    />
                </div>
            </div>
        </div>
    )
}