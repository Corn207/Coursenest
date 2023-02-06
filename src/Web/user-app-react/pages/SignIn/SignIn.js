import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { React, useState } from 'react';
import { Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import classNames from 'classnames/bind';

import styles from './SignIn.module.scss';
import Image from '~/components/Image';
import ImageSliders from '~/components/ImageSliders';
import { adImages } from '~/mockupData/AdsData/AdsData';
import commonImages from '~/assets/images';

const cx = classNames.bind(styles);

function SignIn() {
    const [isShow, setIsShow] = useState(false);

    const handleShowPassword = (event) => {
        event.preventDefault();
        setIsShow(!isShow);
    };

    const {
        register,
        handleSubmit,
        // watch,
        formState: { errors },
    } = useForm({
        defaultValues: {
            userName: '',
            email: '',
        },
    });

    const onSubmit = (data) => {
        console.log(data);
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('login-sidebar')}>
                <div className={cx('sidebar-info')}>
                    <h1>TEAM</h1>
                    <p className={cx('sidebarTitle')}>
                        Hi,
                        <br />
                        Welcome back!
                    </p>
                    <p className={cx('sidebarBody')}>
                        Start 14 days full-featured trial. <br />
                        No credit card required.
                    </p>
                </div>

                <Image className={cx('sidebar-img')} src={commonImages.signInSideBarImg} />
                <p>© 2022 TEAM Inc. All rights reserved.</p>
            </div>

            <div className={cx('login')}>
                <div className={cx('loginInfoTitle')}>
                    <div className={cx('loginDiv')}>
                        <h1 className={cx('loginTitle')}>Sign in</h1>
                    </div>
                    <div className={cx('loginDiv-2')}>
                        <div className={cx('member-yet')}>
                            <p>
                                <strong>Don't have an account? </strong>
                            </p>
                            <p className={cx('sign-up')}>
                                <Link to="/sign-up">Try it here!</Link>
                            </p>
                        </div>
                    </div>
                    <p className={cx('forgot')}>
                        <Link to="/forgot-password">Forgot your password?</Link>
                    </p>
                    {/* <div className={cx('forgot-container')}> */}

                    {/* </div> */}
                </div>

                <form className={cx('loginForm')} onSubmit={handleSubmit(onSubmit)}>
                    <div className={cx('inputContainer')}>
                        <label className={cx('inputTitle')}>Username</label>
                        <span className={cx('form-message')}>{errors.userName?.message}</span>
                        <input
                            className={cx('loginInput')}
                            {...register('userName', {
                                required: 'This input is required',
                            })}
                            style={{ border: errors.userName?.message ? '1px solid red' : '' }}
                            placeholder="Enter your username..."
                        />
                    </div>

                    <div className={cx('inputContainer')}>
                        <label className={cx('inputTitle')}>Password</label>
                        <span className={cx('form-message')}>{errors.password?.message}</span>
                        <div
                            className={cx('passwordInput')}
                            style={{ border: errors.password?.message ? '1px solid red' : '' }}
                        >
                            <input
                                type={isShow ? 'text' : 'password'}
                                className={cx('passwordEnter')}
                                {...register('password', {
                                    required: 'This input is required.',
                                })}
                                placeholder="Enter your password..."
                            />
                            <button className={cx('showButton')} onClick={(event) => handleShowPassword(event)}>
                                <FontAwesomeIcon className={cx('eye-icon')} icon={isShow ? faEyeSlash : faEye} />
                            </button>
                        </div>
                    </div>

                    <button className={cx('loginButton')}>
                        {/* <Link className={cx('link')} to="/home"> */}
                        Log in
                        {/* </Link> */}
                    </button>
                </form>
                <div className={cx('sponsor-info-container')}>
                    <div></div>
                    <p className={cx('sponsor-info')}>*Sponsor by ABC</p>
                </div>
                <div className={cx('containerStyles')}>
                    <ImageSliders images={adImages} />
                </div>
                <div className={cx('desc-info')}>
                    <p className={cx('desc-left')}>Privacy Policy and Tearms of Use</p>
                    <p className={cx('desc-right')}>Developed by Group 3 - GINP17</p>
                </div>
            </div>
        </div>
    );
}

export default SignIn;
