import React, { useState, useEffect, useMemo } from "react";
import "font-awesome/css/font-awesome.min.css";
import "rsuite/dist/rsuite.min.css";
import { Pagination } from "rsuite/";

import styles from "./ManageUsers.module.css";
import DisplayListUser from "../DisplayListUser/DisplayListUser";
import ModalDeleteUser from "../ModalDeleteUser";
import Search from "../Search";
import ModalDetailUser from "../ModalUserDetail/ModalUserDetail";
import instance from "../../api/request";

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

    const fetchListUser = () => {
        instance
            .get(`users/admin?PageNumber=${page}&PageSize=${pageSize}`)
            .then((res) => {
                setListUsers(res.data.queried);
                setCountUser(res.data.count)
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

    const options = [
        { value: '5', text: '5 users/page' },
        { value: '50', text: '50 users/page' },
        { value: '100', text: '100 users/page' }
    ];

    const handleOnChangePageSize = event => {
        console.log(event.target.value);
        setPageSize(parseInt(event.target.value));
    };

    return (
        <div className={styles.container}>
            <div>
                <div className={styles.header}>
                    <Search setKeyWord={setKeyWord} />
                    <div className={styles.userCount}>
                        <span>Total: {countUser} Users</span>
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
