// import { Table, TableBody, TableCell, TableRow } from '@mui/material';
import classNames from 'classnames/bind';

import React, { useEffect, useState } from 'react';
import {
    Checkbox,
    makeStyles,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableFooter,
    TableHead,
    TableRow,
} from '@material-ui/core';

import styles from './Courses.module.scss';
import coursesApi from '~/api/coursesApi';
import axios from 'axios';

const useStyles = makeStyles({
    table: {
        minWidth: 650,
    },
    footer: {
        // display: 'flex',
    },
    backBtn: {
        marginRight: '10',
    },
    nextBtn: {},
});

function createData(id, title, description, status, topic, courseTier, action) {
    return { id, title, description, status, topic, courseTier, action };
}
const data = [
    createData(1, 'Title 1', 'Description 1', 'Status 1', 'Topic 1', 'Course Tier 1', 'Action 1'),
    createData(2, 'Title 2', 'Description 2', 'Status 2', 'Topic 2', 'Course Tier 2', 'Action 2'),
    createData(3, 'Title 3', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(4, 'Title 4', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(5, 'Title 5', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(6, 'Title 6', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(7, 'Title 7', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(8, 'Title 8', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(9, 'Title 9', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(10, 'Title 10', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(11, 'Title 11', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(12, 'Title 12', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(13, 'Title 13', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
    createData(14, 'Title 14', 'Description 3', 'Status 3', 'Topic 3', 'Course Tier 3', 'Action 3'),
];

const cx = classNames.bind(styles);

function Courses() {
    const [data, setData] = useState([]);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [pageSize, setPageSize] = useState(5);

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await axios.get('https://coursenest.corn207.loseyourip.com/courses');
                setData(response.data.queried);
                console.log(response.data.queried);
            } catch (error) {
                console.error(error);
            }
        };
        fetchCourses();
    }, []);

    const handleSelectAll = (event) => {
        if (event.target.checked) {
            setSelected(data.map((item) => item.courseId));
            return;
        }
        setSelected([]);
    };

    const handleSelect = (courseId) => {
        const selectedIndex = selected.indexOf(courseId);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selected, courseId);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selected.slice(1));
        } else if (selectedIndex === selected.length - 1) {
            newSelected = newSelected.concat(selected.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selected.slice(0, selectedIndex), selected.slice(selectedIndex + 1));
        }

        setSelected(newSelected);
    };

    const isSelected = (courseId) => selected.indexOf(courseId) !== -1;

    const handleNextPage = () => {
        setPage(page + 1);
    };

    const handleBackPage = () => {
        setPage(page - 1);
    };

    const handleEditCourse = (courseId) => {
        console.log(courseId);
    };

    const classes = useStyles();
    const currentData = data.slice(page * pageSize, (page + 1) * pageSize);

    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Courses</p>
            <TableContainer component={Paper}>
                <Table className={classes.table} aria-label="custom table">
                    <TableHead>
                        <TableRow>
                            <TableCell padding="checkbox">
                                <Checkbox
                                    indeterminate={selected.length > 0 && selected.length < data.length}
                                    checked={data.length > 0 && selected.length === data.length}
                                    onChange={handleSelectAll}
                                />
                            </TableCell>
                            <TableCell>Title</TableCell>
                            <TableCell align="right">Description</TableCell>
                            <TableCell align="right">Status</TableCell>
                            <TableCell align="right">Topic</TableCell>
                            <TableCell align="right">Course Tier</TableCell>
                            <TableCell align="right">Action</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {currentData.map((row) => (
                            <TableRow key={row.title}>
                                <TableCell>
                                    <input
                                        type="checkbox"
                                        checked={selected.includes(row.courseId)}
                                        onClick={() => handleSelect(row.courseId)}
                                    />
                                </TableCell>
                                <TableCell component="th" scope="row">
                                    {row.title}
                                </TableCell>
                                <TableCell align="right">{row.description}</TableCell>
                                <TableCell align="right">{row.isApproved ? 'Approved' : 'Pending'}</TableCell>
                                <TableCell align="right">{row.topicTitle}</TableCell>
                                <TableCell align="right">{row.tier === 0 ? 'Free' : 'Premium'}</TableCell>
                                <TableCell align="right">
                                    <button onClick={() => handleEditCourse(row.courseId)}>Edit</button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                    <TableFooter>
                        <TableRow className={classes.footer}>
                            <TableCell align="left">*{selected.length} data selected</TableCell>

                            <TableCell className={classes.nextBtn}>
                                {page > 0 && <button onClick={handleBackPage}>Prev</button>}
                                {(page + 1) * pageSize < data.length && <button onClick={handleNextPage}>Next</button>}
                            </TableCell>
                        </TableRow>
                    </TableFooter>
                </Table>
            </TableContainer>
        </div>
    );
}

export default Courses;
