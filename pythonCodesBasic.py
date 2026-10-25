# Python & Selenium Practice Notes
# Cleaned and rearranged from backup notes

# ------------------------------------------------------------
# 1) Count Vowels
# ------------------------------------------------------------
text = "automation"
count = 0

for char in text:
    if char.lower() in "aeiou":
        count += 1

print("Total vowels:", count)

# ------------------------------------------------------------
# 2) Prime Number
# ------------------------------------------------------------
num = 13
is_prime = True

if num < 2:
    is_prime = False
else:
    for i in range(2, num):
        if num % i == 0:
            is_prime = False
            break

if is_prime:
    print(num, "is Prime")
else:
    print(num, "is Not Prime")

# ------------------------------------------------------------
# 3) Find Duplicate Values in a List
# ------------------------------------------------------------
numbers = [1, 2, 3, 2, 5, 1]
seen = set()
duplicates = set()

for num in numbers:
    if num in seen:
        duplicates.add(num)
    else:
        seen.add(num)

print("Duplicate values:", duplicates)

# ------------------------------------------------------------
# 4) Count Frequency of Words
# ------------------------------------------------------------
wordss = "My name is is is Prat Prat"
words = wordss.split()
word_count = {}

for word in words:
    if word in word_count:
        word_count[word] += 1
    else:
        word_count[word] = 1

print(word_count)

# ------------------------------------------------------------
# 5) Fibonacci
# ------------------------------------------------------------
def fibo(n):
    if n < 2:
        return n
    return fibo(n - 1) + fibo(n - 2)

print(fibo(3))

# ------------------------------------------------------------
# 6) Factorial
# ------------------------------------------------------------
def fac(n):
    if n < 2:
        return n
    result = 1
    for i in range(1, n + 1):
        result *= i
    return result

print(fac(5))

# ------------------------------------------------------------
# 7) Duplicate Words in Sentence
# ------------------------------------------------------------
sentence = "My My name name is Prateek is"
words = sentence.split()
seen = set()
duplicate_words = []

for word in words:
    if word in seen:
        duplicate_words.append(word)
    else:
        seen.add(word)

print("Duplicate words:", duplicate_words)
print("Unique words:", seen)

# ------------------------------------------------------------
# 8) Reverse Each Word + Count Frequency
# ------------------------------------------------------------
string = "My name is Prateek am am an having an interview"
words = string.split()
rev_words = []
word_count = {}

for word in words:
    rev_words.append(word[::-1])
    if word in word_count:
        word_count[word] += 1
    else:
        word_count[word] = 1

rev_sentence = " ".join(rev_words)
print(rev_sentence)
print(word_count)

# ------------------------------------------------------------
# 9) Permutations of Each Word
# ------------------------------------------------------------
from itertools import permutations

sentence = "Hi my name is pra"
words = sentence.split()

for word in words:
    combos = set("".join(p) for p in permutations(word))
    print(f"{word} : {len(combos)} : {','.join(combos)}")

# ------------------------------------------------------------
# 10) Reverse Each Word and Count Reversed Words
# ------------------------------------------------------------
sentence = "My name is prateek"
words = sentence.split()
rev_words = []
word_count = {}

for word in words:
    rev_word = word[::-1]
    rev_words.append(rev_word)
    if rev_word in word_count:
        word_count[rev_word] += 1
    else:
        word_count[rev_word] = 1

rev_sentence = " ".join(rev_words)
print(rev_sentence)
print(word_count)

# ------------------------------------------------------------
# 11) Remove Duplicates in One Line
# ------------------------------------------------------------
lst = [2, 2, 22, 2, 3, 44, 555, 5, 5, 5]
print(list(set(lst)))

# ------------------------------------------------------------
# 12) Remove Duplicates, Keep Even Numbers, Square Them
# ------------------------------------------------------------
lst = [2, 2, 2, 3, 3, 3, 4, 5, 5, 4]
unique = list(set(lst))
even = []
squared_even = []

for i in unique:
    if i % 2 == 0:
        even.append(i)
        squared_even.append(i * i)

print(even)
print(squared_even)

# ------------------------------------------------------------
# 13) Sort Numbers Descending
# ------------------------------------------------------------
list_numbers = [2, 4, 3, 5]
list_numbers.sort(reverse=True)
print(list_numbers)

# ------------------------------------------------------------
# 14) String Puzzle: arrange alphabets and numbers
# ------------------------------------------------------------
string = "a7b4c2d6" # output - a2b4c6d7
alpha_chars = ""
nums = []

for ch in string:
    if ch.isalpha():
        alpha_chars += ch
    else:
        nums.append(ch)

alpha = sorted(alpha_chars)
nums.sort()
result = []

for i in range(len(nums)):
    result.append(alpha[i])
    result.append(nums[i])

print("".join(result))

# ------------------------------------------------------------
# 15) Python Basics: Waits in Selenium
# ------------------------------------------------------------
# Implicit wait:
# driver.implicitly_wait(10)
# Global wait for all elements

# Explicit wait:
# wait until specific condition is true

# Fluent wait:
# explicit + pooling interval + ignore exceptions

# ------------------------------------------------------------
# 16) Pytest Notes
# ------------------------------------------------------------
# Basic test structure:
# Setup -> Test logic -> Assertions -> TearDown

# Example fixture
# import pytest
# from selenium import webdriver
#
# @pytest.fixture()
# def driver():
#     driver = webdriver.Chrome()
#     yield driver
#     driver.quit()
#
# def test_login(driver):
#     assert True, "Login test example"

# driver.close()  # closes current tab
# driver.quit()   # closes whole browser window

# Parametrization example
# @pytest.mark.parametrize(
#     "username, password",
#     [("admin", "adminpass"), ("user", "userpass")]
# )
# def test_login(username, password):
#     print(username, password)

# ------------------------------------------------------------
# 17) Fixture and Setup / Teardown
# ------------------------------------------------------------
# Fixture = setup and teardown logic reused across tests
# It keeps tests clean and avoids duplicate code
#
# @pytest.fixture
# def driver():
#     driver = webdriver.Chrome()
#     yield driver
#     driver.quit()

# ------------------------------------------------------------
# 18) Class Method vs Instance Method
# ------------------------------------------------------------
# Instance method belongs to an object and works on instance-specific data.
# Class method belongs to the class itself and is shared across all instances.

# ------------------------------------------------------------
# 19) Conftest in Pytest
# ------------------------------------------------------------
# conftest.py holds common fixtures used across tests.
# Pytest auto-detects it in the project.
#
# Example:
# @pytest.fixture
# def auto_login():
#     client = {"url": "google.com"}
#     yield client
#
# def test_login(auto_login):
#     assert auto_login["url"] == "google.com", "Wrong url"

# Failed tests command:
# pytest --lf

# ------------------------------------------------------------
# 20) Custom Markers
# ------------------------------------------------------------
# @pytest.mark.smoke
# pytest -m smoke
#
# @pytest.mark.skip(reason="not ready")
# @pytest.mark.skipif(condition, reason="")

# ------------------------------------------------------------
# 21) Fixture Scope
# ------------------------------------------------------------
# Function -> runs before each test function
# Class -> runs once per test class
# Module -> runs once per module/file
# Session -> runs once for the whole test session

# ------------------------------------------------------------
# 22) Selenium Basic Commands
# ------------------------------------------------------------
# driver.back()
# driver.forward()
# driver.refresh()
# driver.save_screenshot()

# ------------------------------------------------------------
# 23) Dropdown in Selenium
# ------------------------------------------------------------
# from selenium.webdriver.support.ui import Select
# dropdown = Select(driver.find_element(By.ID, "country"))
# dropdown.select_by_visible_text("India")
# dropdown.select_by_value("IN")
# dropdown.select_by_index(1)

# ------------------------------------------------------------
# 24) Checkbox in Selenium
# ------------------------------------------------------------
# checkbox.click()
# checkbox.is_selected()

# ------------------------------------------------------------
# 25) Alerts in Selenium
# ------------------------------------------------------------
# alert = driver.switch_to.alert
# alert.dismiss()
# alert.accept()
# alert.send_keys("hi")

# ------------------------------------------------------------
# 26) Multiple Windows
# ------------------------------------------------------------
# driver.switch_to.window(window_handles[1])

# ------------------------------------------------------------
# 27) ActionChains in Selenium
# ------------------------------------------------------------
# from selenium.webdriver.common.action_chains import ActionChains
# action = ActionChains(driver)
# action.move_to_element(element).perform()
# action.drag_and_drop(source, target).perform()
# action.context_click(element).perform()

# ------------------------------------------------------------
# 28) JavaScript Executor
# ------------------------------------------------------------
# driver.execute_script("window.scrollBy(0,500)")

# ------------------------------------------------------------
# 29) Basic Pytest + POM Example
# ------------------------------------------------------------
# conftest.py
# @pytest.fixture
# def driver():
#     driver = webdriver.Chrome()
#     yield driver
#     driver.quit()

# Page/login_page.py
# from selenium.webdriver.common.by import By
#
# class Login:
#     USERNAME = (By.ID, "Username")
#     PASSWORD = (By.ID, "Password")
#
#     def __init__(self, driver):
#         self.driver = driver
#
#     def enter_username(self, username):
#         self.driver.find_element(*self.USERNAME).send_keys(username)
#
#     def enter_password(self, password):
#         self.driver.find_element(*self.PASSWORD).send_keys(password)
#
#     def click_login(self):
#         self.driver.find_element(By.ID, "submit").click()
#
#     def login(self, username, password):
#         self.enter_username(username)
#         self.enter_password(password)
#         self.click_login()

# tests/test_login.py
# import pytest
# from Page.login_page import Login
#
# @pytest.mark.parametrize(
#     "username, password",
#     [("admin", "passwordadmin"), ("User1", "userpass")]
# )
# def test_login(driver, username, password):
#     driver.get("https://google.com")
#     login_page = Login(driver)
#     login_page.login(username, password)
#     assert "Dashboard" in driver.title

# ------------------------------------------------------------
# 30) Python is Synchronous by Default
# ------------------------------------------------------------
# Python executes code line by line unless async/await is used.
# Async programming requires the asyncio module.
#
# Example:
# async def hello():
#     print("Hello")

# ------------------------------------------------------------
# 31) Final Quick Revision Notes
# ------------------------------------------------------------
# - Python basics: loops, list, set, dict, functions
# - Selenium: browser automation, waits, click, send_keys, dropdowns
# - Pytest: fixtures, markers, parametrization
# - POM: Page object model for cleaner tests
# - Playwright: modern browser automation, async style

# ------------------------------------------------------------
# 32) Extra Example: Login Flow In Pytest Style
# ------------------------------------------------------------
# import pytest
# from selenium import webdriver
#
# @pytest.fixture
# def driver():
#     driver = webdriver.Chrome()
#     yield driver
#     driver.quit()
#
# def test_login(driver):
#     driver.get("https://example.com")
#     assert "Example" in driver.title

