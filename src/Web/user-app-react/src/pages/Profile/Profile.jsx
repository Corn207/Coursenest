import { useEffect, useState } from 'react';
import axios from 'axios';
import styles from './Profile.module.scss';
import './Profile.module.scss';
import images from '~/assets/images';
import Modal from 'react-bootstrap/Modal';
import Achievement from '~/components/Achievement/Achievement';

function Profile() {
    const [userInfo, setUserInfo] = useState({});
    const [showModalAchievement, setShowModalAchievement] = useState(false);
    const [avatar, setAvatar] = useState(images.userAvatar);
    // const [image, setImage] = useState();

    useEffect(() => {
        axios
            // .get("http://192.168.0.3:21002/users/1/profile")
            // .get('http://localhost:3000/profile')
            .get('http://localhost:21002/users/1/profile')
            .then((res) => {
                setUserInfo(res.data);
                if (res.data.avatar.uri !== null) setAvatar(res.data.avatar.uri);
            })
            .catch((err) => console.log(err));
    }, []);

    // const handleSubmit = async (event) => {
    //     event.preventDefault();
    //     const formData = new FormData();
    //     formData.append('image', image);

    //     const headers = {
    //         userId: 1
    //     };

    //     axios
    //         .put('http://localhost:21002/users/me/cover', formData, { headers })
    //         .then((res) => {
    //             console.log(res);
    //         })
    //         .catch((err) => console.log(err));
    // };

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                {/* test change avatar */}
                {/* <form onSubmit={handleSubmit}>
                    <input type="file" onChange={(e) => setImage(e.target.files[0])} />
                    <button type="submit">Upload</button>
                </form> */}
                <div className={styles.avatarContainer}>
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
                        <p className={styles.editBtn}>Edit</p>
                    </div>
                    <p>{userInfo.aboutMe}</p>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Basic Information</h2>
                        <p className={styles.editBtn}>Edit</p>
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
                                        <img src={images.experience} alt="" className={styles.experienceImage} />
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
export default Profile;
