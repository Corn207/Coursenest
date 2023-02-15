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

import styles from './PublisherCourses.module.scss';
import axios from 'axios';
import { useParams } from 'react-router-dom';

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

const cx = classNames.bind(styles);

function PublisherCourses() {
    const [data, setData] = useState([]);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [pageSize, setPageSize] = useState(5);

    let params = useParams();

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await axios.get('https://coursenest.corn207.loseyourip.com/courses', {
                    params: {
                        PublisherUserId: params.PublisherUserId,
                    },
                });
                setData(response.data.queried);
                console.log(response.data.queried);
                console.log(params.PublisherUserId);
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

export default PublisherCourses;
