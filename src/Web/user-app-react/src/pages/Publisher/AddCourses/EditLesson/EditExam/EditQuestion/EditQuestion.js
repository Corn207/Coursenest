import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import axios from 'axios';
import classNames from 'classnames/bind';
import { useState } from 'react';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';

import styles from './EditQuestion.module.scss';

const cx = classNames.bind(styles);

function EditQuestion({ examTitle, chosenQuestion, handleBackStep }) {
    const [questions, setQuestions] = useState([chosenQuestion]);
    const [question, setQuestion] = useState(chosenQuestion);
    const [answers, setAnswers] = useState([]);
    const [questionEditTitle, setQuestionEditTitle] = useState(questions[0].content);
    const [isEditingTitle, setIsEditingTitle] = useState(false);

    const handleTitleChange = (event) => {
        setQuestionEditTitle(event.target.value);
        setQuestion({ ...question, content: event.target.value });
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleAnswerChange = (questionIndex, answerIndex) => {
        setAnswers((prevAnswers) => {
            const newAnswers = [...prevAnswers];
            newAnswers[questionIndex] = answerIndex;
            return newAnswers;
        });
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            const newQuestion = { ...newQuestionList[questionIndex] };
            newQuestion.choices = newQuestion.choices.map((choice, index) => {
                return {
                    ...choice,
                    isCorrect: index === answerIndex,
                };
            });
            newQuestionList[questionIndex] = newQuestion;
            return newQuestionList;
        });
    };

    const handleAddAnswer = (questionIndex) => {
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            newQuestionList[questionIndex] = {
                ...newQuestionList[questionIndex],
                choices: [
                    ...newQuestionList[questionIndex].choices,
                    {
                        content: 'new answer',
                        isCorrect: false,
                    },
                ],
            };
            return newQuestionList;
        });
    };

    const handleChoiceContentEdit = (questionIndex, choiceIndex, newContent) => {
        setQuestions((prevQuestionList) => {
            const questionToUpdate = prevQuestionList[questionIndex];
            const updatedChoices = [...questionToUpdate.choices];
            const choiceToUpdate = updatedChoices[choiceIndex];
            choiceToUpdate.content = newContent;
            questionToUpdate.choices = updatedChoices;
            const updatedQuestionList = [...prevQuestionList];
            updatedQuestionList[questionIndex] = questionToUpdate;
            return updatedQuestionList;
        });
    };

    const handleChoiceClick = (questionIndex, choiceIndex) => {
        const newContent = window.prompt(
            'Enter new content of the answer:',
            questions[questionIndex].choices[choiceIndex].content,
        );
        if (newContent !== null) {
            handleChoiceContentEdit(questionIndex, choiceIndex, newContent);
        }
    };

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    return (
        <div className={cx('wrapper')}>
            <p className={cx('examTitle')}>{examTitle}</p>
            <ul className={cx('body')}>
                {questions.map((item, questionIndex) => (
                    <li className={cx('question')} key={questionIndex}>
                        <div className={cx('questionHeader')}>
                            {isEditingTitle ? (
                                <input
                                    type="text"
                                    className={cx('titleInput')}
                                    value={questionEditTitle}
                                    onChange={handleTitleChange}
                                    onBlur={handleTitleBlur}
                                    autoFocus
                                />
                            ) : (
                                <p className={cx('questionName')} onClick={handleTitleClick}>
                                    Question {questionIndex + 1}: {question.content}
                                </p>
                            )}
                        </div>
                        <ul className={cx('answerList')}>
                            {item.choices.map((choice, answerIndex) => (
                                <li key={answerIndex} className={cx('answerLi')}>
                                    <div className={cx('answerDiv')}>
                                        <label className={cx('answerLabel')}>
                                            <input
                                                className={cx('answerInput')}
                                                type="radio"
                                                name={`question-${questionIndex}`}
                                                value={choice.content}
                                                checked={answers[questionIndex] === answerIndex}
                                                onChange={() => handleAnswerChange(questionIndex, answerIndex)}
                                            />
                                            <span onClick={() => handleChoiceClick(questionIndex, answerIndex)}>
                                                {choice.content}
                                            </span>
                                        </label>
                                        <p className={cx('delete')}>Delete</p>
                                    </div>
                                </li>
                            ))}
                        </ul>
                        <button className={cx('addAnswerBtn')} onClick={() => handleAddAnswer(questionIndex)}>
                            <FontAwesomeIcon icon={faPlus} />
                        </button>
                        {/*  */}
                    </li>
                ))}
            </ul>
            <CancelConfirmBtns onCancel={handleCancel} />
        </div>
    );
}

export default EditQuestion;
