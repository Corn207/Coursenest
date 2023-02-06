import React, { useState, useEffect, useMemo } from "react";
import axios from "axios";
import "font-awesome/css/font-awesome.min.css";
import "rsuite/dist/rsuite.min.css";
import { Pagination } from "rsuite/";

import styles from "./ManageUsers.module.css";
import DisplayListUser from "../DisplayListUser/DisplayListUser";
import ModalDeleteUser from "../ModalDeleteUser";
import Search from "../Search";
import ModalDetailUser from "../ModalUserDetail/ModalUserDetail";

export default function ManageUsers() {
    const [listUsers, setListUsers] = useState([]);
    const [countUser, setCountUser] = useState();
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(5);

    const [keyword, setKeyWord] = useState();
    const [showModalDeleteUser, setShowModalDeleteUser] = useState(false);
    const [deletedData, setDeletedData] = useState({});
    const [showModalUpdateUser, setShowModalUpdateUser] = useState(false);
    const [dataNeedUpdate, setDataNeedUpdate] = useState({});

    useEffect(() => {
        fetchListUser();
    }, [page, pageSize]);

    useEffect(() => {
        handleCountUser();
    }, []);

    const fetchListUser = () => {
        axios
            // .get("http://192.168.0.3:21002/users")
            .get(`http://localhost:21002/users?Page=${page - 1}&PageSize=${pageSize}`)
            .then((res) => {
                setListUsers(res.data);
            })
            .catch((err) => console.log(err));
    };

    const handleCountUser = () => {
        axios
            .get("http://localhost:21002/users/count")
            .then((res) => {
                setCountUser(res.data);
            })
            .catch((err) => console.log(err));
    };

    function getFilteredListUser() {
        if (!keyword) {
            return listUsers;
        }
        return listUsers.filter((user) => {
            return (
                user.fullName.toLowerCase().indexOf(keyword.toLowerCase()) !== -1
            );
        });
    }

    var filteredListUser = useMemo(getFilteredListUser, [keyword, listUsers]);

    const handleClickUpdateUser = (user) => {
        setShowModalUpdateUser(true);
        setDataNeedUpdate(user);
    };

    const handleClickDelUser = (user) => {
        setShowModalDeleteUser(true);
        setShowModalUpdateUser(false);
        setDeletedData(user);
    };

    const handleOnChangePage = (event) => {
        setPage(parseInt(event));

    };

    const handleOnChangePageSize = (event) => {
        setPageSize(parseInt(event));
    }

    return (
        <div className={styles.container}>
            <div>
                <div className={styles.header}>
                    <Search setKeyWord={setKeyWord} />
                    <div className={styles.userCount}>
                        <span>Total: {countUser} Users</span>
                    </div>
                    <div>
                        <select onChange={handleOnChangePageSize}>
                            <option>5</option>
                            <option>50</option>
                            <option>100</option>
                            <option>500</option>
                        </select>
                    </div>
                </div>
                    <DisplayListUser
                        listUsers={filteredListUser}
                        // handleClickDelUser={handleClickDelUser}
                        handleClickUpdateUser={handleClickUpdateUser}
                    />
                    <ModalDetailUser
                        show={showModalUpdateUser}
                        setShow={setShowModalUpdateUser}
                        fetchListUser={fetchListUser}
                        dataNeedUpdate={dataNeedUpdate}
                        handleClickDelUser={handleClickDelUser}
                    />
                    <ModalDeleteUser
                        show={showModalDeleteUser}
                        setShow={setShowModalDeleteUser}
                        deletedData={deletedData}
                        fetchListUser={fetchListUser}
                    />
            </div>

            <div className={styles.pagination}>
                <div className={styles.center}>
                    <Pagination
                        prev
                        last
                        next
                        first
                        size="lg"
                        total={countUser}
                        limit={pageSize}
                        maxButtons={5}
                        activePage={page}
                        onChangePage={handleOnChangePage}
                    />
                </div>
            </div>

        </div>
    );
}
