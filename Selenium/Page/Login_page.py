from selenium.webdriver.common.by import By
import time

class Login:
    def __init__(self, driver):
        self.driver = driver

    EMAIL = (By.XPATH, "//input[@aria-label='Email address']")
    PASSWORD = (By.XPATH, "//input[@aria-label='Password']")
    LOGIN_BUTTON = (By.XPATH, "//button[normalize-space()='LOGIN']")

    def open_login_page(self):
        self.driver.get("https://automation-pm.onrender.com/auth/login")

    def enter_email(self, email):
        element = self.driver.find_element(*self.EMAIL)
        element.clear()
        element.send_keys(email)

    def enter_password(self, password):
        element = self.driver.find_element(*self.PASSWORD)
        element.clear()
        element.send_keys(password)

    def click_login(self):
        self.driver.find_element(*self.LOGIN_BUTTON).click()

    def login(self, email, password):
        time.sleep(2)
        self.enter_email(email)
        time.sleep(2)
        self.enter_password(password)
        time.sleep(2)
        self.click_login()
        