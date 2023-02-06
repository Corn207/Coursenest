import classNames from 'classnames/bind';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { adImages } from '~/mockupData/AdsData/AdsData';
import ImageSliders from '~/components/ImageSliders';
import styles from './Forgot.module.scss';

const cx = classNames.bind(styles);

function Forgot() {
    const {
        register,
        handleSubmit,
        // watch,
        formState: { errors },
    } = useForm({
        defaultValues: {
            email: '',
        },
    });

    const notify = () => {
        toast.success('Email has been sent successfully.', {
            position: toast.POSITION.TOP_CENTER,
            className: 'toast-success',
        });
    };

    const onSubmit = (data) => {
        console.log(data);
        notify();
    };

    return (
        <div className={cx('wrapper')}>
            <ToastContainer />
            <div className={cx('forgot-form-container')}>
                <div className={cx('forgot-form')}>
                    <div className={cx('form-header')}>
                        <h1>TEAM</h1>
                        <p>Forgot the password</p>
                    </div>
                    <form className={cx('email-fill-form')} onSubmit={handleSubmit(onSubmit)}>
                        <div className={cx('email-fill')}>
                            <label>Enter your email address and we'll send you a link to reset your password.</label>
                            <span className={cx('form-message')}>{errors.email?.message}</span>
                            <input
                                type="email"
                                className={cx('registerInput')}
                                {...register('email', {
                                    required: 'This input is required.',
                                    pattern: {
                                        // eslint-disable-next-line no-useless-escape
                                        value: /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                        message: 'Invalid email!',
                                    },
                                })}
                                style={{ border: errors.email?.message ? '1px solid red' : '' }}
                                placeholder="Enter your email..."
                            />
                        </div>
                        <button className={cx('confirmButton')}>Confirm</button>
                    </form>
                    <div className={cx('member-yet')}>
                        <p>Already have account?</p>
                        <p className={cx('signInUp-btn')}>
                            <Link className={cx('login-link')} to={'/sign-in'}>
                                Login?
                            </Link>
                        </p>
                        <p className={cx('signInUp-btn')}>
                            <Link className={cx('signUp-link')} to={'/sign-up'}>
                                Sign Up?
                            </Link>
                        </p>
                    </div>
                    <div className={cx('sponsor-info-container')}>
                        <div></div>
                        <p className={cx('sponsor-info')}>*Sponsor by ABC</p>
                    </div>
                    <div className={cx('containerStyles')}>
                        <ImageSliders images={adImages} />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Forgot;
