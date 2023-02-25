import { useEffect, useState } from 'react';
import styles from './Profile.module.scss';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Achievement from '~/components/Achievement/Achievement';
import avatarImg from '../../assets/images/user-avatar.png';
import experienceImg from '../../assets/images/experience.png';
import config from '~/config';
import axios from 'axios';

export default function Profile() {

    const userId = localStorage.getItem("userId")
    const token = localStorage.getItem("accessToken");

    const [userInfo, setUserInfo] = useState({});
    const [userInfoNeedUpdate, setUserInfoNeedUpdate] = useState({
        "email": "",
        "phonenumber": "",
        "fullName": "",
        "title": "",
        "aboutMe": "",
        "location": ""
    });
    const [aboutMe, setAboutMe] = useState({"aboutMe": ""})

    const [showModalAchievement, setShowModalAchievement] = useState(false);
    const [showModalChangeAvatar, setShowModalChangeAvatar] = useState(false);
    const [showModalEditInfo, setShowModalEditInfo] = useState(false);
    const [showModalEditAboutMe, setShowModalEditAboutMe] = useState(false);

    const [avatar, setAvatar] = useState(avatarImg);
    const [file, setFile] = useState(null);
    const [preview, setPreview] = useState();


    useEffect(() => {
        fetchInfoUser();
    }, []);

    const fetchInfoUser = () => {
        axios
            .get(`${config.baseUrl}/api/users/${userId}`)
            .then((res) => {
                setUserInfo(res.data);
                if (res.data.avatar != null) setAvatar(res.data.avatar.uri);
                console.log(res.data);
                setUserInfoNeedUpdate({                
                    "email": res.data.email,
                    "phonenumber": res.data.phonenumber,
                    "fullName": res.data.fullName,
                    "title": res.data.title,
                    "aboutMe": res.data.aboutMe,
                    "location": res.data.location    
                })
                setAboutMe({"aboutMe": res.data.aboutMe});
            })
            .catch((err) => console.log(err));
    }

    // change avatar
    const handleNewImage = (e) => {
        setFile(e.target.files[0]);
        const objectUrl = URL.createObjectURL(e.target.files[0]);
        setPreview(objectUrl);
    }

    const handleClickSaveChangeAvatar = () => {
        const formData = new FormData();
        formData.append("formFile", file);
        axios
            .put(`${config.baseUrl}/api/users/me/cover`, formData, {
                headers: {
                    'Content-Type': "multipart/form-data",
                    'Authorization': `Bearer ${token}`
                }
            }
            )
            .then(() => {
                fetchInfoUser();
                setShowModalChangeAvatar(false);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    // update basic info
    const handleClickEditInfo = () => {
        console.log(userInfoNeedUpdate);
        setShowModalEditInfo(true);
    }

    const handleChangeInfo = (event) => {
        const value = event.target.value;
        setUserInfoNeedUpdate({
            ...userInfoNeedUpdate,
            [event.target.name]: value
        });
    }

    const handleConfirmUpdateInfo = (event) => {
        // test
        // const userInfoNeedUpdate = { "dateOfBirth": "2023-02-13T13:30:20-05:00" }
        event.preventDefault();
        console.log(userInfoNeedUpdate);
        axios
            .put(`${config.baseUrl}/api/users/me`, userInfoNeedUpdate, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then((res) => {
                console.log(res.data);
                setShowModalEditInfo(false);
                fetchInfoUser();
            })
            .catch((err) => {
                console.log(err);
            })

    }

    // update about me info
    const handleConfirmUpdateAboutMe = (event) => {
        event.preventDefault();
        console.log(aboutMe);
        axios
            .put(`${config.baseUrl}/api/users/me`, aboutMe, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
            .then(() => {
                setShowModalEditAboutMe(false);
                fetchInfoUser();
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const handleChangeAboutMe = (event) => {
        const value = event.target.value;
        console.log(aboutMe);
        setAboutMe({"aboutMe": value});
    }

    const handleClickEditAboutMe = (data) => {
        console.log(data);
        setShowModalEditAboutMe(true);
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
                        <p className={styles.editBtn} onClick={() => handleClickEditAboutMe(userInfo.aboutMe)}>Edit</p>
                    </div>
                    <p>{userInfo.aboutMe}</p>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Basic Information</h2>
                        <p className={styles.editBtn} onClick={handleClickEditInfo}>Edit</p>
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
                <Modal.Body style={{display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 30}} >
                    <img style={{width: 200, height: 200}} src={preview} />
                    <input name="newImage" type="file" onChange={handleNewImage} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={handleClickSaveChangeAvatar}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
            <Modal
                show={showModalEditAboutMe}
                onHide={() => setShowModalEditAboutMe(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>About Me</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={handleConfirmUpdateAboutMe} style={{paddingLeft: 50, paddingRight: 50}}>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>About me:</label>
                            <textarea 
                                cols="70" 
                                rows="10"  
                                type="text"
                                name="aboutMe"
                                value={aboutMe?.aboutMe}
                                onChange={handleChangeAboutMe}>
                            </textarea>
                        </div>
                        <div>
                            <input type='submit' style={{ color: "white", padding: 5, backgroundColor: "blue", borderRadius: 5 }} />
                        </div>
                    </form>
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalEditInfo}
                onHide={() => setShowModalEditInfo(false)}
                backdrop="static"
                keyboard={false}
                size="md"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>Edit Infomations</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={handleConfirmUpdateInfo} style={{paddingLeft: 50, paddingRight: 50}}>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Email:</label>
                            <input
                                type="text"
                                name="email"
                                value={userInfoNeedUpdate.email}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Phone Number:</label>
                            <input
                                type="text"
                                name="phonenumber"
                                value={userInfoNeedUpdate.phonenumber}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Full Name:</label>
                            <input
                                type="text"
                                name="fullName"
                                value={userInfoNeedUpdate.fullName}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Title:</label>
                            <input
                                type="text"
                                name="title"
                                value={userInfoNeedUpdate.title}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>About me:</label>
                            <input
                                type="text"
                                name="aboutMe"
                                value={userInfoNeedUpdate.aboutMe}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Gender: </label>
                            <select onChange={handleChangeInfo}>
                                <option value="grapefruit">Male</option>
                                <option value="lime">Female</option>
                            </select>
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Date of birth:</label>
                            <input type="date"/>
                        </div>
                        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: 20}}>
                            <label>Location:</label>
                            <input
                                type="text"
                                name="location"
                                value={userInfoNeedUpdate.location}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div>
                            <input type='submit' style={{ color: "white", padding: 5, backgroundColor: "blue", borderRadius: 5 }} />
                        </div>
                    </form>
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalAchievement}
                onHide={() => setShowModalAchievement(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
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