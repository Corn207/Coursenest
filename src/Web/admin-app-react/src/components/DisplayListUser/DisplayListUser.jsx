import styles from "./DisplayListUser.module.css";
import "font-awesome/css/font-awesome.min.css";
import avatarDefault from '../../assets/avatar.png';
import GetRolesByUserId from "../GetRolesByUserId";
// import instance from "../../api/request";
// import { useMemo, useState } from "react";

function DisplayListUser(props) {
    const {listUsers, handleClickUpdateUser} = props;

    // const [newListUsers, setNewListUsers] = useState();

    // const test = useMemo(async () => {
    //     await Promise.all(
    //         listUsers.map(async (user) => {
    //             const res = await instance.get(`roles/${user.userId}`);
    //             setNewListUsers(res.data)
    //         })
    //     );
    // }, [listUsers]);
   
    // console.log(test)
    // console.log(newListUsers)

    return(
        <div>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">Name</th>
                        <th scope="col">Email address</th>
                        <th scope="col">Roles</th>
                        <th scope="col">Action</th>
                    </tr>
                </thead>
                <tbody>
                    {listUsers && listUsers.map((user) => {
                        return (
                            <tr className={styles.tableRow} key={user.userId}>
                                <td className={styles.userId} scope="row">
                                    <p>{user.userId}</p>
                                </td>
                                <td className={styles.avatarWrapper}>
                                    <img src={user.avatar?.uri ?? avatarDefault} className={styles.avatar} alt=""/>
                                    <div>
                                        <p className={styles.fullname}>{user.fullName}</p>
                                        <p>{user.username}</p>
                                    </div>
                                </td>
                                <td className={styles.email}>
                                    <p>{user.email}</p>
                                </td>
                                
                                <td>
                                    <GetRolesByUserId roles={user.roles}/>
                                </td>
                                <td>
                                    <button
                                        className={`btn btn-secondary btn-sm ${styles.action}`}
                                        onClick={() => handleClickUpdateUser(user)}
                                    >
                                        <i className="fa fa-edit"></i>
                                    </button>
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
}

export default DisplayListUser;