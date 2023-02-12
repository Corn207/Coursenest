import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './AddCourses.module.scss';

const cx = classNames.bind(styles);

function AddCourses() {
    const [image, setImage] = useState(null);

    const handleChange = (e) => {
        setImage(URL.createObjectURL(e.target.files[0]));
    };

    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Web Design Course</p>
            <div className={cx('contentContainer')}>
                <div className={cx('imageDiv')}>
                    <div className={cx('imageActionDiv')}>
                        <p className={cx('fileTitle')}>Course Image</p>
                        <input type="file" onChange={handleChange} />
                    </div>
                    {image && <img className={cx('imageContainer')} src={image} alt={'course'}></img>}
                </div>
                <div className={cx('courseInfoDiv')}>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Title</p>
                        <input className={cx('input')} type={'text'}></input>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Description</p>
                        <input className={cx('input')} type={'text'}></input>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Tier</p>
                        <div className={cx('courseTierDiv')}>
                            <p className={cx('courseTier')}>Free</p>
                            <p className={cx('courseTier')}>Premium</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default AddCourses;
