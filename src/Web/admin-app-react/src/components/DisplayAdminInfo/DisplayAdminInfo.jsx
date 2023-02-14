import { useEffect, useState } from 'react';
import instance from '../../api/request';
import styles from './DisplayAdminInfo.module.css';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Achievement from '../Achievement/Achievement';
import avatarImg from '../../assets/avatar.png';
import experienceImg from '../../assets/experience.png';

export default function DisplayAdminInfo() {

    const userId = localStorage.getItem("userId")

    const [userInfo, setUserInfo] = useState({});
    // const [userInfoNeedUpdate, setUserInfoNeedUpdate] = useState({});

    const [showModalAchievement, setShowModalAchievement] = useState(false);
    const [showModalChangeAvatar, setShowModalChangeAvatar] = useState(false);
    const [showModalEditInfo, setShowModalEditInfo] = useState(false);

    const [avatar, setAvatar] = useState(avatarImg);
    const [file, setFile] = useState(null);
    const [preview, setPreview] = useState();


    useEffect(() => {
        fetchInfoUser();
    }, []);

    const fetchInfoUser = () => {
        instance
        .get(`users/${userId}`)
        .then((res) => {
            setUserInfo(res.data);
            if (res.data.avatar != null) setAvatar(res.data.avatar.uri);
        })
        .catch((err) => console.log(err));
    }

    const handleNewImage = (e) => {
        setFile(e.target.files[0]);
        const objectUrl = URL.createObjectURL(e.target.files[0]);
        setPreview(objectUrl);
    }
    
    const handleClickSaveChangeAvatar = () => {
        const formData = new FormData();
        formData.append("formFile", file);
        instance
            .put(`users/me/cover`, formData, {
                headers: {
                    'Content-Type': "multipart/form-data"
                }
            })
            .then(() => {
                fetchInfoUser();
                setShowModalChangeAvatar(false);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const handleClickEditInfo = () => {
        setShowModalEditInfo(true);
        console.log(userInfo);
    }

    const handleConfirmUpdateInfo = () => {
        // setUserInfoNeedUpdate({"dateOfBirth": "2023-02-13T13:30:20-05:00"})
        // console.log(userInfoNeedUpdate)
        const userInfoNeedUpdate = { "dateOfBirth": "2023-02-13T13:30:20-05:00" }
        instance
            .put(`users/me`, userInfoNeedUpdate)
            .then((res) => {
                console.log(res.data);
                setShowModalEditInfo(false);
            })
            .catch((err) => {
                console.log(err);
            })

    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <div className={styles.avatarContainer} onClick={() => setShowModalChangeAvatar(true)}>
                    <img className={styles.roundAvatar} src={avatar} alt="" />
                    <div>
                        <div className={styles.middle}></div>
                        <p className={styles.text}>Edit</p>
                    </div>
                </div>
                <div className={styles.rightHeader}>
                    <h2>{userInfo.fullName}</h2>
                    <p className={styles.jobTitleLabel}>Job Title</p>
                    <p>{userInfo.title}</p>
                </div>
            </div>

            <div className={styles.body}>
                <div>
                    {/* Achievements la enrollment completed */}
                    <div className={styles.heading}>
                        <h2>Achievements</h2>
                        <p className={styles.seeAllBtn} onClick={() => setShowModalAchievement(true)}>
                            SEE ALL
                        </p>
                    </div>
                    <div className={styles.listAchievement}>
                        {userInfo.achievements &&
                            userInfo.achievements.slice(0, 6).map((achievement) => {
                                return <Achievement key={achievement.achievementId} achievement={achievement} />;
                            })}
                    </div>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>About Me</h2>
                        <p className={styles.editBtn} onClick={handleClickEditInfo}>Edit</p>
                    </div>
                    <p>{userInfo.aboutMe}</p>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Basic Information</h2>
                        {/* <p className={styles.editBtn}>Edit</p> */}
                    </div>
                    <table className={styles.tableInfo}>
                        <tbody>
                            <tr>
                                <td>Gender</td>
                                <td className={styles.tableRightContent}>
                                    {userInfo.gender === 1 ? 'female' : userInfo.gender === 0 ? 'male' : 'N/A'}
                                </td>
                            </tr>
                            <tr>
                                <td>Date of Birth</td>
                                <td className={styles.tableRightContent}>
                                    {userInfo.dateOfBirth == null
                                        ? 'N/A'
                                        : new Date(userInfo.dateOfBirth).toDateString()}
                                </td>
                            </tr>
                            <tr>
                                <td>Phone number</td>
                                <td className={styles.tableRightContent}>{userInfo.phonenumber == null ? 'N/A' : userInfo.phonenumber}</td>
                            </tr>
                            <tr>
                                <td>Email</td>
                                <td className={styles.tableRightContent}>{userInfo.email}</td>
                            </tr>
                            <tr>
                                <td>Location</td>
                                <td className={styles.tableRightContent}>{userInfo.location == null ? 'N/A' : userInfo.location}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Work Experiences</h2>
                        <p className={styles.editBtn}>Edit</p>
                    </div>
                    <div className={styles.listExperience}>
                        {userInfo.experiences &&
                            userInfo.experiences.map((experience) => {
                                return (
                                    <div key={experience.experienceId} className={styles.experience}>
                                        <img src={experienceImg} alt="" className={styles.experienceImage} />
                                        <div className={styles.experienceContent}>
                                            <h5>{experience.name}</h5>
                                            <p>{experience.title}</p>
                                        </div>
                                    </div>
                                );
                            })}
                    </div>
                </div>
            </div>

            <Modal
                show={showModalChangeAvatar}
                onHide={() => setShowModalChangeAvatar(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>Change Avatar</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body className={styles.newAvatar}>
                    <img src={preview} />
                    <input name="newImage" type="file" onChange={handleNewImage} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={handleClickSaveChangeAvatar}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal
                show={showModalEditInfo}
                onHide={() => setShowModalEditInfo(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h2>Edit Infomations</h2>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <label>Email</label>
                        <textarea>{userInfo.email}</textarea>
                    </div>
                    <div>
                        <label>Phone Number</label>
                        <textarea>{userInfo.phonenumber}</textarea>
                    </div>
                    <div>
                        <label>Full Name</label>
                        <textarea>{userInfo.fullName}</textarea>
                    </div>
                    <div>
                        <label>Title</label>
                        <textarea>{userInfo.title}</textarea>
                    </div>
                    <div>
                        <label>About Me</label>
                        <textarea>{userInfo.aboutMe}</textarea>
                    </div>
                    <div>
                        <label>Gender</label>
                        <textarea>{userInfo.gender}</textarea>
                    </div>
                    <div>
                        <label>Date of birth</label>
                        <textarea>{userInfo.dateOfBirth}</textarea>
                    </div>
                    <div>
                        <label>Location</label>
                        <textarea>{userInfo.location}</textarea>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={handleConfirmUpdateInfo}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal
                show={showModalAchievement}
                onHide={() => setShowModalAchievement(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h2>Achievements</h2>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body style={{ display: 'flex', gap: 20, flexWrap: 'wrap', justifyContent: 'center' }}>
                    {userInfo.achievements &&
                        userInfo.achievements.map((achievement) => {
                            return <Achievement key={achievement.achievementId} achievement={achievement} />;
                        })}
                </Modal.Body>
            </Modal>
        </div>
    );
}