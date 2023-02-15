import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import styles from './ModalUserDetail.module.css'
import avatarDefault from '../../assets/avatar.png';
import { useState } from 'react';
import instance from "../../api/request";

function ModalDetailUser(props) {
    const { show, setShow, fetchListUser, dataNeedUpdate, handleClickDelUser } = props;    

    const handleClose = () => setShow(false);

    const [newPassword, setNewPassword] = useState('');

    const handleResetPwd = () => {
        console.log(dataNeedUpdate.userId);
        instance
            .put(`authenticate/reset-password`, dataNeedUpdate.userId)
            .then((res) => {
                console.log(res.data);
                setNewPassword(res.data);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const handleUpdateUser = () => {
        handleClose();
        fetchListUser();
    };
    
    return (
        <>
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
                size="xm"
            >
                <Modal.Header closeButton>
                    User Detail
                </Modal.Header>

                <Modal.Body>
                    <div className={styles.userInfo}>
                        <div>
                            <img src={dataNeedUpdate.avatar?.uri ?? avatarDefault} className={styles.avatar} alt=""/> 
                        </div>
                        <div className={styles.userInfoLeft}>
                            <h3 className={styles.fullName}>{dataNeedUpdate.fullName}</h3>
                            <p>Username: {dataNeedUpdate.username}</p>
                            <p>Email: {dataNeedUpdate.email}</p>
                        </div>
                    </div>
                    <div>
                        <Button 
                            className={styles.resetPsw}
                            variant="info"
                            onClick={() => {
                                handleResetPwd();
                            }}
                        >
                            Reset Password
                        </Button>
                        <br /><br />
                        <p>New Password: {newPassword}</p>
                        {/* <input type='text' value={newPassword} onFocus={e => e.target.select()} /> */}
                    </div> 
                    <div className={styles.roles}>
                        <h5>Roles</h5>
                        <div className={styles.roundedCorner}>
                            <span>Student</span>
                            <span>0 day</span>
                            <input type="date" className={styles.noOutline} />
                            {/* install https://ant-design.antgroup.com/components/date-picker */}
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Instructor</span>
                            <span>1 day</span>
                            <input type="date" className={styles.noOutline} />
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Publisher</span>
                            <span>0 day</span>
                            <input type="date" className={styles.noOutline} />
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Admin</span>
                            <span>0 day</span>
                            <input type="date" className={styles.noOutline} />
                        </div>
                    </div>
                    <Button 
                        className={styles.delUser} 
                        variant="danger"
                        onClick={() => { 
                            handleClickDelUser(dataNeedUpdate);
                        }}
                    >
                        Delete User
                    </Button>
                </Modal.Body>

                <Modal.Footer>
                    <Button 
                        variant="secondary" 
                        onClick={handleClose}
                    >
                        Cancel
                    </Button>
                    <Button 
                        variant="success"
                        onClick={() => {
                            handleUpdateUser();
                        }}
                    >
                        Save
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ModalDetailUser;