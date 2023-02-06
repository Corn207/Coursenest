import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import images from '~/assets/images';

import styles from './UserAction.module.scss';

// sửa lại component user button + guest button thành 1 component
// check login -> show user button / guest button

export default function UserActions() {
    const [isOpen, setIsOpen] = useState(false);
    const user = {id: 1, username: "linh"}
    return (
        <div>
            <style>{`
                .hidden {
                    visibility: hidden;
                }
                .show {
                    display: block;
                }
            `}</style>

            <div onClick={() => setIsOpen(!isOpen)} className={styles.bdropdown}>
                <img src={images.reviewer3} alt=""/>
                <p>{user.username}</p>
                <img src={images.dropDownIcon} alt=""/>
            </div>
            
            <div className={isOpen ? 'show' : 'hidden'}>
                <div className={styles.ddcontent}>
                    <Link to="/profile" className={`textlink ${styles.dditem}`}>
                        <p>My Profile</p>
                    </Link>
                    <Link to="#" className={`textlink ${styles.dditem}`}>
                        <p>Subscription</p>
                    </Link>
                    <Link to="#" className={`textlink ${styles.dditem}`}>
                        <p>Sign Out</p>
                    </Link>
                </div>
            </div>
        </div>
    );
}
