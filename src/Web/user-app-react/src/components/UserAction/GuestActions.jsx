import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import guesticon from '../../../assets/guest-icon.png';
import dropdownicon from '../../../assets/arrow-down.png';

import styles from './UserAction.module.scss';

export default function GuestActions() {
    // const [isOpen, setIsOpen] = useState(false);
    // const toggling = () => setIsOpen(!isOpen);
    // return (
    //     <div>
    //         <div onClick={toggling} className={styles.bdropdown}>
    //             <img src={guesticon} />
    //             <p>Guest</p>
    //             <img src={dropdownicon} />
    //         </div>
    //         {isOpen && (
    //             <div className={styles.ddcontent}>
    //                 <Link to="/sign-up" className={`${styles.textlink} ${styles.dditem}`}>
    //                     <p>Sign Up</p>
    //                 </Link>
    //                 <Link to="/sign-in" className={`${styles.textlink} ${styles.dditem}`}>
    //                     <p>Sign In</p>
    //                 </Link>
    //             </div>
    //         )}

    //     </div>
    // );
    const [isOpen, setIsOpen] = useState(false);
    return (
        <div>
            <div onClick={() => setIsOpen(!isOpen)} className={styles.bdropdown}>
                <img src={guesticon} alt="" />
                <p>Guest</p>
                <img src={dropdownicon} alt="" />
            </div>
            <style>{`
                .hidden {
                    visibility: hidden;
                }
                .show {
                    display: block;
                }
            `}</style>
            <div className={isOpen ? 'show' : 'hidden'}>
                <div className={styles.ddcontent}>
                    <Link to="#" className={`textlink ${styles.dditem}`}>
                        <p>Sign Up</p>
                    </Link>
                    {/* <hr /> */}
                    <Link to="#" className={`textlink ${styles.dditem}`}>
                        <p>Sign In</p>
                    </Link>
                </div>
            </div>
        </div>
    );
}
